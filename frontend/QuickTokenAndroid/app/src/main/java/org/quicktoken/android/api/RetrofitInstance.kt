package org.quicktoken.android.api

import okhttp3.logging.HttpLoggingInterceptor
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

object RetrofitInstance {
    private val logInterceptor: HttpLoggingInterceptor
        get() = HttpLoggingInterceptor().apply { level = HttpLoggingInterceptor.Level.BODY }

    private val httpClient
        get() = OkHttpClient.Builder()
            .addNetworkInterceptor(logInterceptor)
//            .addInterceptor(QuickTokenApiKeyInterceptor())
            .build()

    private val retrofit
        get() = Retrofit.Builder()
            .client(httpClient)
            .addConverterFactory(GsonConverterFactory.create())
            .baseUrl(API_URL)
            .build()

    val QUICK_TOKEN_API: QuickTokenApi
        get() = retrofit.create(QuickTokenApi::class.java)
}