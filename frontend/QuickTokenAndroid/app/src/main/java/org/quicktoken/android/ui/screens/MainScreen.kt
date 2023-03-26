package org.quicktoken.android.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.material.Button
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Scaffold
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
import androidx.navigation.NavController
import kotlinx.coroutines.launch
import org.quicktoken.android.viewmodel.MainViewModel

@Composable
fun MainScreen(
    viewModel: MainViewModel,
    navController: NavController,
) {

    val portfolioBar = "Portfolio"
    val tradingBar = "Trading"
    var currentBar by rememberSaveable {
        mutableStateOf(portfolioBar) // show portfolio by default
    }

    Scaffold(
        modifier = Modifier.fillMaxSize(),
        backgroundColor = MaterialTheme.colors.background,
        topBar = {
            Column(
                modifier = Modifier.fillMaxWidth(),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.Top,
            ) {
                Text(
                    text = "QuickToken",
                    modifier = Modifier,
                    color = MaterialTheme.colors.primary,
                    fontSize = 32.sp,
                    fontWeight = FontWeight.ExtraBold,
                    fontFamily = FontFamily.SansSerif,
                )
                if (viewModel.session.approvedAccounts()?.isNotEmpty() == true) {
                    Text(
                        text = viewModel.session.approvedAccounts()!!.first(),
                        modifier = Modifier,
                        color = MaterialTheme.colors.secondary,
                        fontSize = 16.sp,
                        fontWeight = FontWeight.Medium,
                    )
                }
            }
        },
        bottomBar = {
            val bars = listOf(portfolioBar, tradingBar)
            Row(
                modifier = Modifier
                    .fillMaxWidth()
                    .background(Color.Black)
                    .padding(20.dp),
                horizontalArrangement = Arrangement.SpaceEvenly,
                verticalAlignment = Alignment.Bottom,
            ) {
                val coroutineScope = rememberCoroutineScope()
                for (bar in bars) {
                    Button(
                        onClick = {
                            coroutineScope.launch {
                                viewModel.fetchBalanceData()
                                viewModel.fetchBalanceHistory()
                                viewModel.fetchDexAssets()
                            }
                            currentBar = bar
                        },
                    ) {
                        Text(
                            text = bar,
                        )
                    }
                }
            }
        },
    ) { paddingValues ->
        val uiState by viewModel.uiState.collectAsState()
        if (currentBar == portfolioBar) {
            PortfolioScreen(
                viewModel = viewModel,
                uiState = uiState,
                navController = navController,
                paddingValues = paddingValues,
            )
        } else if (currentBar == tradingBar) {
            TradingScreen(
                viewModel = viewModel,
                uiState = uiState,
                navController = navController,
                paddingValues = paddingValues,
            )
        }
    }
}