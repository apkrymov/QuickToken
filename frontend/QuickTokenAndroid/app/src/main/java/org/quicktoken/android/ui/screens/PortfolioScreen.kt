package org.quicktoken.android.ui.screens

import androidx.compose.foundation.layout.*
import androidx.compose.material.CircularProgressIndicator
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import com.himanshoe.charty.line.CurveLineChart
import kotlinx.coroutines.launch
import org.quicktoken.android.ui.AssetsColumn
import org.quicktoken.android.utils.SmartContractAddresses
import org.quicktoken.android.utils.Web3Utils
import org.quicktoken.android.viewmodel.MainViewModel
import java.math.BigInteger

@Composable
fun PortfolioScreen(
    viewModel: MainViewModel,
    uiState: MainViewModel.MainUiState,
    navController: NavController,
    paddingValues: PaddingValues
) {

    LaunchedEffect(Unit) {
        this.launch {
            viewModel.fetchBalanceData()
            viewModel.fetchBalanceHistory()
        }
    }

    Column(
        modifier = Modifier.padding(paddingValues),
    ) {
        Column(
            modifier = Modifier.padding(10.dp),
            verticalArrangement = Arrangement.SpaceEvenly,
            horizontalAlignment = Alignment.Start,
        ) {
            Text(
                text = "Current balance",
                color = MaterialTheme.colors.primary,
                fontSize = 26.sp,
                fontWeight = FontWeight.ExtraBold,
                fontFamily = FontFamily.SansSerif,
            )
            Row(
                modifier = Modifier
                    .padding(start = 10.dp, end = 10.dp)
                    .fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
            ) {
                Text(
                    text = "${uiState.balance?.currency} QTKC",
                    color = MaterialTheme.colors.secondary,
                    fontSize = 28.sp,
                    fontWeight = FontWeight.Medium,
                    fontFamily = FontFamily.Monospace,
                )
                Text(
                    text = "${uiState.balance?.eth} ETH",
                    color = MaterialTheme.colors.secondary,
                    fontSize = 28.sp,
                    fontWeight = FontWeight.Medium,
                    fontFamily = FontFamily.Monospace,
                )
            }
        }

        Column {
            Column(
                modifier = Modifier.padding(10.dp),
                verticalArrangement = Arrangement.SpaceEvenly
            ) {
                Text(
                    text = "Portfolio",
                    color = MaterialTheme.colors.primary,
                    fontSize = 26.sp,
                    fontWeight = FontWeight.ExtraBold,
                    fontFamily = FontFamily.SansSerif,
                )
            }

            Column(
                modifier = Modifier
                    .heightIn(0.dp, 250.dp)
                    .padding(30.dp)
                    .fillMaxWidth(),
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally,
            ) {
                if (uiState.balanceHistory?.isNotEmpty() == true) {
                    CurveLineChart(
                        lineData = viewModel.getPortfolioData(),
                        chartColor = Color.Black,
                        lineColor = List(100) { Color.Green },
                        modifier = Modifier
                            .fillMaxWidth()
                    )
                } else {
                    CircularProgressIndicator(
                        modifier = Modifier
                            .fillMaxWidth()
                    )
                }
            }
        }

        Row(
            modifier = Modifier
                .padding(10.dp)
                .fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween,
        ) {
            Text(
                text = "01.03.2023",
                color = Color.Green,
                fontSize = 16.sp,
            )
            Text(
                text = "31.03.2023",
                color = Color.Green,
                fontSize = 16.sp,
            )
        }

        Column(
            modifier = Modifier.padding(5.dp)
        ) {
            Text(
                text = "Current Total Profit: ${uiState.balance?.currency} QTKС",
                color = MaterialTheme.colors.secondary,
                fontSize = 18.sp,
                fontWeight = FontWeight.Normal,
                fontFamily = FontFamily.Monospace,
            )
            if (uiState.balanceHistory?.isNotEmpty() == true) {
                Text(
                    text = "Expected Profit: ${viewModel.getPortfolioData().last().yValue} QTKС",
                    color = MaterialTheme.colors.secondary,
                    fontSize = 18.sp,
                    fontWeight = FontWeight.Normal,
                    fontFamily = FontFamily.Monospace,
                )
            }
        }

        val context = LocalContext.current
        uiState.balance?.assets?.let {
            AssetsColumn(
                assets = it,
                buttonActionName = "Sell",
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
                            Web3Utils.encodeSell(formattedAssetId),
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
}