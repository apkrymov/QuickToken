package org.quicktoken.android

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.tooling.preview.Preview
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import org.quicktoken.android.ui.screens.AssetScreen
import org.quicktoken.android.ui.screens.AuthScreen
import org.quicktoken.android.ui.screens.MainScreen
import org.quicktoken.android.ui.theme.QuickTokenAndroidTheme
import org.quicktoken.android.viewmodel.MainViewModel

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {

            val navController = rememberNavController()

            val viewModel: MainViewModel = viewModel()
            // sharing viewModel between auth and main screens
            // because need to store session for sending transactions

            QuickTokenAndroidTheme {
                NavHost(
                    navController = navController,
                    startDestination = "auth_screen",
                ) {
                    composable("auth_screen") {
                        AuthScreen(
                            viewModel,
                            navController,
                        )
                    }
                    composable("main_screen") {
                        MainScreen(
                            viewModel,
                            navController,
                        )
                    }
                    composable("asset_screen/?assetId={assetId}/?price={price}/?dir={dir}/?timestamp={timestamp}") {
                        val assetArgument = it.arguments?.getString("assetId")
                        val priceArgument = it.arguments?.getString("price")
                        val dirArgument = it.arguments?.getString("dir")
                        val timestampArgument = it.arguments?.getString("timestamp")
                        AssetScreen(
                            assetArgument,
                            priceArgument,
                            dirArgument,
                            timestampArgument,
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun Greeting(name: String) {
    Text(text = "Hello $name!")
}

@Preview(showBackground = true)
@Composable
fun DefaultPreview() {
    QuickTokenAndroidTheme {
        Greeting("Android")
    }
}