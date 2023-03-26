package org.quicktoken.android.dto

data class DexAsset(
    val burn_timestamp: String,
    val daily_interest_rate: Int,
    val id: String,
    val in_stock: Int,
    val price: Int,
    var is_approved: Boolean = false,
)
