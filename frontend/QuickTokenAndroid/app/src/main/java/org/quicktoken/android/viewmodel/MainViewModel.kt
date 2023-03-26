package org.quicktoken.android.viewmodel

import android.content.Context
import android.content.Intent
import android.net.Uri
import android.util.Log
import androidx.lifecycle.ViewModel
import com.himanshoe.charty.line.model.LineData
import com.squareup.moshi.Moshi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.flow.*
import okhttp3.OkHttpClient
import org.komputing.khex.extensions.toHexString
import org.quicktoken.android.dto.Balance
import org.quicktoken.android.dto.BalanceHistoryItem
import org.quicktoken.android.dto.DexAsset
import org.quicktoken.android.repo.QuickTokenRepository
import org.quicktoken.android.repo.QuickTokenRepositoryMocked
import org.quicktoken.android.server.BridgeServer
import org.walletconnect.Session
import org.walletconnect.impls.*
import org.walletconnect.nullOnThrow
import org.web3j.tx.gas.DefaultGasProvider
import java.io.File
import java.math.BigInteger
import java.util.*

class MainViewModel(
    private val quickTokenRepository: QuickTokenRepository = QuickTokenRepositoryMocked(),
) : ViewModel() {

    private val tag: String = "MainViewModel"

    // State of Main Screen
    data class MainUiState(
        var isWalletConnected: Boolean = false,
        var userWallet: String = "",
        var balance: Balance? = null,
        var dexAssets: List<DexAsset>? = null,
        var balanceHistory: List<BalanceHistoryItem>? = null,
        var portfolioData: List<LineData> = emptyList(),
    )

    // State flow of MainUiState
    private val _uiState = MutableStateFlow(MainUiState())
    val uiState: StateFlow<MainUiState> = _uiState.asStateFlow()

    private var client: OkHttpClient = OkHttpClient.Builder().build()
    private var moshi: Moshi = Moshi.Builder().build()

    private lateinit var bridge: BridgeServer
    private lateinit var storage: WCSessionStore

    lateinit var session: Session
    private lateinit var config: Session.Config
    private lateinit var activeCallback: Session.Callback

    fun startWalletConnectSession(context: Context) {
        bridge = BridgeServer(moshi)
        bridge.start()
        storage = FileWCSessionStore(
            File(context.cacheDir, "session_store.json").apply {
                createNewFile()
            },
            moshi
        )
    }

    private fun resetSession() {
        nullOnThrow { session }?.clearCallbacks()
        val key = ByteArray(32).also { Random().nextBytes(it) }.toHexString(prefix = "")
        config = Session.Config(
            UUID.randomUUID().toString(),
            "https://bridge.walletconnect.org",
            key
        )
        session = WCSession(
            config?.toFullyQualifiedConfig() ?: return,
            MoshiPayloadAdapter(moshi),
            storage,
            OkHttpTransport.Builder(client, moshi),
            Session.PeerMeta(
                url = "https://quicktoken.org",
                name = "QuickToken",
                description = "QuickToken for Android",
                icons = arrayListOf()
            )
        )
        session?.offer()
    }

    fun disconnectWallet() {
        Log.e(tag, "disconnect wallet")
        session?.kill()
        _uiState.update { uiState ->
            uiState.copy(isWalletConnected = false)
        }
    }

    fun connectWallet(context: Context) {
        resetSession()
        activeCallback = object : Session.Callback {
            override fun onMethodCall(call: Session.MethodCall) = Unit

            override fun onStatus(status: Session.Status) {
                status.handleStatus()
            }
        }
        session?.addCallback(activeCallback ?: return)
        context.startActivity(Intent(Intent.ACTION_VIEW).apply {
            data = Uri.parse(config?.toWCUri() ?: return)
        })
    }

    fun Session.Status.handleStatus() {
        when (this) {
            Session.Status.Approved -> sessionApproved()
            Session.Status.Closed -> sessionClosed()
            Session.Status.Connected -> {
                _uiState.update { uiState ->
                    uiState.copy(isWalletConnected = true)
                }
            }
            Session.Status.Disconnected -> {
                _uiState.update { uiState ->
                    uiState.copy(isWalletConnected = false)
                }
            }
            is Session.Status.Error,
            -> Log.e(tag, "WC Session => Status handleStatus: $this")
        }
    }

    private fun sessionApproved() {
        val address = session?.approvedAccounts()?.firstOrNull() ?: return
        /* Provider name*/
        // val walletType = session?.peerMeta()?.name ?: ""
        _uiState.update {
            it.copy(userWallet = address)
        }
    }

    private fun sessionClosed() {

    }

    fun sendTransaction(context: Context, encodedFunction: String, toAddress: String) {
        val from = session.approvedAccounts()?.first() ?: return
        val txRequest = System.currentTimeMillis()
        Log.i(tag, "onStart: from:${from}")
        session.performMethodCall(
            Session.MethodCall.SendTransaction(
                id = txRequest,
                from = from,
                to = toAddress,
                nonce = "0x0114",
                gasPrice = DefaultGasProvider.GAS_PRICE.toString(16),
                gasLimit = DefaultGasProvider.GAS_LIMIT.toString(16),
                value = BigInteger.ZERO.toString(16),
                data = encodedFunction,
            ),
        ) { }
        val intent = Intent(Intent.ACTION_VIEW)
        intent.data = Uri.parse("wc:")
        intent.flags = Intent.FLAG_ACTIVITY_NEW_TASK
        context.startActivity(intent)
    }

    suspend fun fetchBalanceData() {
        quickTokenRepository.getBalance()
            .flowOn(Dispatchers.IO)
            .catch { e ->
                e.message?.let { Log.d(tag, it) }
            }
            .collect {
                _uiState.update { uiState ->
                    uiState.copy(balance = it)
                }
            }
    }

    suspend fun fetchBalanceHistory() {
        quickTokenRepository.getBalanceHistory()
            .flowOn(Dispatchers.IO)
            .catch { e ->
                e.message?.let { Log.d(tag, it) }
            }
            .collect {
                _uiState.update { uiState ->
                    uiState.copy(balanceHistory = it)
                }
            }
    }

    fun getPortfolioData(): List<LineData> {
        val currentBalanceHistory = uiState.value.balanceHistory
        val portfolioResult = mutableListOf<LineData>()
        if (currentBalanceHistory != null) {
            for (i in currentBalanceHistory.indices) {
                portfolioResult.add(
                    LineData(
                        i,
                        currentBalanceHistory[i].currency.toFloat(),
                    )
                )
            }
        }
        return portfolioResult
    }

    suspend fun fetchDexAssets() {
        quickTokenRepository.getDexOffers()
            .flowOn(Dispatchers.IO)
            .catch { e ->
                e.message?.let { Log.d(tag, it) }
            }
            .collect {
                _uiState.update { uiState ->
                    uiState.copy(dexAssets = it)
                }
            }
    }
}