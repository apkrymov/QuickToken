package org.quicktoken.android.ui.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.himanshoe.charty.line.CurveLineChart
import com.himanshoe.charty.line.model.LineData
import java.lang.Math.abs
import java.text.SimpleDateFormat
import java.util.*

@Composable
fun AssetScreen(
    assetId: String?,
    price: String?,
    dir: String?,
    timestamp: String?
) {
    val dateFormat = SimpleDateFormat("dd-MM-yyyy", Locale.getDefault())
    val currentDate = Date()

    if (assetId == null || timestamp == null || dir == null || price == null) {
        Text(
            text = "Unable to get package data!",
            color = MaterialTheme.colors.secondary
        )
    } else {
        val assetDate = dateFormat.parse(timestamp)
        val difference = kotlin.math.abs(assetDate.time - currentDate.time)
        val differenceDates = (difference / (24 * 60 * 60 * 1000)).toInt()
        Column(
            modifier = Modifier
                .background(MaterialTheme.colors.background)
                .fillMaxSize(),
            verticalArrangement = Arrangement.Top,
            horizontalAlignment = Alignment.Start,
        ) {

            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.Center,
            ) {
                Text(
                    text = assetId,
                    modifier = Modifier,
                    color = MaterialTheme.colors.secondary,
                    fontSize = 16.sp,
                    fontWeight = FontWeight.Medium,
                )
            }

            var newPrice = price.toFloat()
            val lineData = List(differenceDates) {
                newPrice += newPrice.div(100).times(dir.toFloat())
                LineData(
                    it,
                    newPrice
                )
            }
            CurveLineChart(
                modifier = Modifier
                    .heightIn(0.dp, 300.dp)
                    .padding(30.dp),
                lineData = lineData,
                chartColor = Color.Black,
                lineColor = List(differenceDates) { Color.Green },
            )
            Row(
                modifier = Modifier
                    .padding(10.dp)
                    .fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
            ) {
                Text(
                    text = dateFormat.format(currentDate),
                    color = Color.Green
                )
                Text(
                    text = timestamp.substring(0, 10),
                    color = Color.Green
                )
            }
            AssetInfo(infoName = "Price: ", info = price.toString())
            AssetInfo(infoName = "Daily Interest Rate: ", info = dir.toString())
            AssetInfo(infoName = "Total Days: ", info = differenceDates.toString())
            AssetInfo(infoName = "Total Profit: ", info = (newPrice - price.toFloat()).toString())
            val totalProfitInPercentage =
                kotlin.math.abs((price.toFloat() - newPrice).div(price.toFloat()).times(100))
            AssetInfo(infoName = "Total Profit %: ", info = totalProfitInPercentage.toString())

            var total = 0F
            for (point in lineData) {
                total += point.yValue
            }
            AssetInfo(
                infoName = "Arithmetic Mean: ",
                info = total.div(lineData.size).toString()
            )
        }
    }
}

@Composable
fun AssetInfo(infoName: String, info: String) {
    Row(
        modifier = Modifier
            .padding(10.dp)
            .fillMaxWidth(),
        horizontalArrangement = Arrangement.SpaceBetween,
    ) {
        Text(
            text = infoName,
            color = MaterialTheme.colors.primary,
            fontSize = 18.sp,
            fontWeight = FontWeight.ExtraBold,
            fontFamily = FontFamily.SansSerif,
        )
        Text(
            text = info,
            color = MaterialTheme.colors.secondary,
            fontSize = 18.sp,
            fontWeight = FontWeight.Normal,
            fontFamily = FontFamily.Monospace,
        )
    }
}