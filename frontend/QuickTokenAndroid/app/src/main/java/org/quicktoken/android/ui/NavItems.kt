package org.quicktoken.android.ui

sealed class NavItem(
    val title: String,
    val icon: Int,
    val screen_route: String
) {
    object Home : NavItem("Home", 0, "home_screen")
}