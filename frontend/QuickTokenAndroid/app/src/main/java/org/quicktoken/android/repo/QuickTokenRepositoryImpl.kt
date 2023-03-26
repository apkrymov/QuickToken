package org.quicktoken.android.repo

import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow
import org.quicktoken.android.api.RetrofitInstance
import org.quicktoken.android.dto.AssetInfoItem
import org.quicktoken.android.dto.Balance
import org.quicktoken.android.dto.BalanceHistoryItem
import org.quicktoken.android.dto.DexAsset

class QuickTokenRepositoryImpl : QuickTokenRepository {

    override fun getAuthToken(address: String): Flow<String> = flow {
        emit(
            RetrofitInstance.QUICK_TOKEN_API.getAuthToken(address)
        )
    }

    override fun getBalance(): Flow<Balance> = flow {
        emit(
            RetrofitInstance.QUICK_TOKEN_API.getBalance()
        )
    }

    override fun getBalanceHistory(): Flow<List<BalanceHistoryItem>> = flow {
        emit(
            RetrofitInstance.QUICK_TOKEN_API.getBalanceHistory()
        )
    }

    override fun getAssetInfo(id: Int): Flow<List<AssetInfoItem>> = flow {
        emit(
            RetrofitInstance.QUICK_TOKEN_API.getAssetInfo(id)
        )
    }

    override fun getDexOffers(): Flow<List<DexAsset>> = flow {
        emit(
            RetrofitInstance.QUICK_TOKEN_API.getDexAssets()
        )
    }
}
