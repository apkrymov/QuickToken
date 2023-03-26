package org.quicktoken.android.ui

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.border
import androidx.compose.foundation.combinedClickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.Button
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Text
import androidx.compose.runtime.*
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import org.quicktoken.android.dto.DexAsset

@OptIn(ExperimentalFoundationApi::class)
@Composable
fun AssetsColumn(
    assets: List<DexAsset>,
    buttonActionName: String,
    onAssetClick: (assetId: String, price: Int, dir: Int, timestamp: String) -> Unit,
    onAssetActionClick: (assetId: String, approved: Boolean) -> Unit,
    paddingValues: PaddingValues,
) {
    LazyColumn(
        verticalArrangement = Arrangement.SpaceEvenly,
    ) {
        items(assets) {
            Row(
                modifier = Modifier
                    .padding(16.dp)
                    .combinedClickable(
                        onClick = { },
                        onLongClick = {
                            onAssetClick(
                                it.id,
                                it.price,
                                it.daily_interest_rate,
                                it.burn_timestamp,
                            )
                        },
                    )
                    .fillMaxWidth()
                    .border(2.dp, Color.Gray),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                var isApproved by rememberSaveable { mutableStateOf(false) }

                Column(
                    modifier = Modifier.padding(5.dp),
                    horizontalAlignment = Alignment.Start,
                    verticalArrangement = Arrangement.SpaceEvenly,
                ) {
                    Text(
//                        text = "Asset: ${it.id}",
                        text = if (it.id.length > 10) {
                            "Asset: ${it.id.substring(0, 10)}..."
                        } else {
                            "Asset: ${it.id}"
                        },
                        modifier = Modifier.padding(start = 5.dp),
                        color = MaterialTheme.colors.primary,
                        fontSize =  16.sp, // 16.sp,
                        fontWeight = FontWeight.ExtraBold,
                        fontFamily = FontFamily.SansSerif,
                    )
                    Text(
                        text = "Daily Interest Rate: ${it.daily_interest_rate}%",
                        modifier = Modifier.padding(start = 5.dp, top = 5.dp),
                        color = MaterialTheme.colors.secondary,
                        fontSize = 12.sp,
                        fontWeight = FontWeight.Light,
                        fontFamily = FontFamily.Monospace,
                    )
                    Text(
                        text = "Will burn at ${it.burn_timestamp}",
                        modifier = Modifier.padding(start = 5.dp, top = 5.dp),
                        color = MaterialTheme.colors.secondary,
                        fontSize = 12.sp,
                        fontWeight = FontWeight.Light,
                        fontFamily = FontFamily.Monospace,
                    )
                }
                Column(
                    modifier = Modifier.padding(5.dp),
                    horizontalAlignment = Alignment.End,
                ) {
                    val scope = rememberCoroutineScope()
                    Button(
                        modifier = Modifier.padding(end = 5.dp),
                        onClick = {
                            if (!isApproved) {
                                scope.launch {
                                    delay(2000)
                                    isApproved = true
                                }
                            }
                            onAssetActionClick(
                                it.id,
                                isApproved,
                            )
                        }
                    ) {
                        if (isApproved) {
                            Text(
                                buttonActionName
                            )
                        } else {
                            Text(
                                "Approve $buttonActionName"
                            )
                        }
                    }
                    Text(
                        text = "Price: ${it.price} QTKC",
                        modifier = Modifier.padding(end = 15.dp),
                        color = MaterialTheme.colors.secondary,
                        fontSize = 12.sp,
                        fontWeight = FontWeight.Light,
                        fontFamily = FontFamily.Monospace,
                    )
                    Text(
                        text = "In Stock: ${it.in_stock}",
                        modifier = Modifier.padding(end = 15.dp),
                        color = MaterialTheme.colors.secondary,
                        fontSize = 12.sp,
                        fontWeight = FontWeight.Light,
                        fontFamily = FontFamily.Monospace,
                    )
                }
            }
        }
    }
}