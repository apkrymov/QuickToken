package org.quicktoken.android.api

import org.quicktoken.android.dto.*
import retrofit2.http.GET

const val API_URL = ""

interface QuickTokenApi {

    @GET("/api/v1/auth/wallet")
    suspend fun getAuthToken(address: String): String

    @GET("/api/v1/account/balance")
    suspend fun getBalance(): Balance

    @GET("/api/v1/account/history")
    suspend fun getBalanceHistory(): List<BalanceHistoryItem>

    @GET("/api/v1/asset/serial")
    suspend fun getAssetInfo(id: Int): List<AssetInfoItem>

    @GET("/api/v1/dex/")
    suspend fun getDexAssets(): List<DexAsset>
}