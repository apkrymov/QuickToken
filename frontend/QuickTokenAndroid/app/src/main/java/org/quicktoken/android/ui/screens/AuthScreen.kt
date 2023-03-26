package org.quicktoken.android.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Button
import androidx.compose.material.ButtonDefaults
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import org.quicktoken.android.viewmodel.MainViewModel

@Composable
fun AuthScreen(
    viewModel: MainViewModel,
    navController: NavController,
) {

    val uiState by viewModel.uiState.collectAsState()

    val authContext = LocalContext.current.also {
        viewModel.startWalletConnectSession(it)
    }

    if (uiState.isWalletConnected) {
        LaunchedEffect(key1 = Unit) {
            navController.navigate("main_screen")
        }
    }

    Column(
        modifier = Modifier
            .background(MaterialTheme.colors.background)
            .fillMaxSize(),
        verticalArrangement = Arrangement.SpaceEvenly,
        horizontalAlignment = Alignment.CenterHorizontally,
    ) {

        Column(
            modifier = Modifier,
            verticalArrangement = Arrangement.SpaceEvenly,
            horizontalAlignment = Alignment.CenterHorizontally,
        ) {
            Text(
                text = "QuickToken",
                modifier = Modifier.padding(),
                color = MaterialTheme.colors.primary,
                fontSize = 64.sp,
                fontWeight = FontWeight.ExtraBold,
                fontFamily = FontFamily.SansSerif,
            )
            Text(
                text = "Please connect your wallet",
                modifier = Modifier.padding(top = 25.dp),
                color = Color.Red,
                fontSize = 18.sp,
                fontWeight = FontWeight.Medium,
                fontFamily = FontFamily.Monospace,
            )
            Button(
                modifier = Modifier
                    .padding(top = 100.dp)
                    .clip(RoundedCornerShape(15.dp))
                    .width(128.dp)
                    .height(64.dp),
                onClick = {
                    when (uiState.isWalletConnected) {
                        false -> {
                            viewModel.connectWallet(authContext)
                        }
                        true -> {
                            viewModel.disconnectWallet()
                        }
                    }
                },
                colors = ButtonDefaults.buttonColors(
                    backgroundColor = MaterialTheme.colors.primary,
                    contentColor = Color.Black
                )
            ) {
                Text(
                    text = if (uiState.isWalletConnected) "Disconnect Wallet" else "Connect",
                    style = MaterialTheme.typography.button
                )
            }
        }
    }
}
