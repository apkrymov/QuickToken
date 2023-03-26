package org.quicktoken.android.ui.screens

import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.platform.LocalContext
import androidx.navigation.NavController
import kotlinx.coroutines.launch
import org.quicktoken.android.ui.AssetsColumn
import org.quicktoken.android.utils.SmartContractAddresses
import org.quicktoken.android.utils.Web3Utils
import org.quicktoken.android.viewmodel.MainViewModel
import java.math.BigInteger

@Composable
fun TradingScreen(
    viewModel: MainViewModel,
    uiState: MainViewModel.MainUiState,
    navController: NavController,
    paddingValues: PaddingValues,
) {
    LaunchedEffect(Unit) {
        this.launch { viewModel.fetchDexAssets() }
    }

    val context = LocalContext.current
    uiState.dexAssets?.let {
        AssetsColumn(
            assets = it,
            buttonActionName = "Buy",
            onAssetClick = { assetId, price, dir, timestamp ->
                navController.navigate("asset_screen/?assetId=${assetId}/?price=${price}/?dir=${dir}/?timestamp=${timestamp}")
            },
            onAssetActionClick = { assetId, approved ->
                val formattedAssetId = BigInteger(
                    assetId.replace("-", ""),
                    16
                )
                if (approved) {
                    viewModel.sendTransaction(
                        context,
                        Web3Utils.encodeBuy(formattedAssetId),
                        SmartContractAddresses.DEX_CONTRACT
                    )
                } else {
                    viewModel.sendTransaction(
                        context,
                        Web3Utils.encodeApprove(formattedAssetId),
                        SmartContractAddresses.QTKA_CONTRACT
                    )
                }
            },
            paddingValues = paddingValues,
        )
    }
}