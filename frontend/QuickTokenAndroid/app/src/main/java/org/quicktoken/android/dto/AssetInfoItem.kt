package org.quicktoken.android.dto

data class AssetInfoItem(
    val burn_timestamp: String,
    val daily_interest_rate: Int,
    val id: String,
    val owners: Map<String, Int>,
    val price: Int
)
