package org.quicktoken.android.repo

import kotlinx.coroutines.flow.Flow
import org.quicktoken.android.dto.*

interface QuickTokenRepository {

    fun getAuthToken(address: String): Flow<String>

    fun getBalance(): Flow<Balance>

    fun getBalanceHistory(): Flow<List<BalanceHistoryItem>>

    fun getAssetInfo(id: Int): Flow<List<AssetInfoItem>>

    fun getDexOffers(): Flow<List<DexAsset>>
}