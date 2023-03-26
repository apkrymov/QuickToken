package org.quicktoken.android.dto

data class Balance(
    val assets: List<DexAsset>,
    val currency: String,
    val eth: String
)
