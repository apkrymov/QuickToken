package org.quicktoken.android.repo

import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow
import org.quicktoken.android.dto.*

class QuickTokenRepositoryMocked : QuickTokenRepository {

    override fun getAuthToken(address: String): Flow<String> {
        TODO("Not yet implemented")
    }

    override fun getBalance(): Flow<Balance> = flow {
        emit(
            Balance(
                currency = "20000",
                eth = "0",
                assets = listOf(
                    DexAsset(
                        burn_timestamp = "31-03-2023",
                        daily_interest_rate = 10,
                        id = "2ace1824-f14d-4bf6-b374-e467073ad90",
                        in_stock = 1,
                        price = 50,
                    ),
                    DexAsset(
                        burn_timestamp = "31-04-2023",
                        daily_interest_rate = 10,
                        id = "e9a1a742-d266-4d9d-81d9-933bb0a164bd",
                        in_stock = 2,
                        price = 100,
                    ),
                ),
            )
        )
    }

    override fun getBalanceHistory(): Flow<List<BalanceHistoryItem>> = flow {
        emit(
            listOf(
                BalanceHistoryItem(
                    currency = "0",
                    eth = "0",
                    timestamp = "07-03-2023"
                ),
                BalanceHistoryItem(
                    currency = "20000",
                    eth = "0",
                    timestamp = "08-03-2023"
                ),
                BalanceHistoryItem(
                    currency = "180119",
                    eth = "0",
                    timestamp = "31-03-2023"
                ),
            )
        )
    }

    override fun getAssetInfo(id: Int): Flow<List<AssetInfoItem>> = flow {
        emit(
            listOf(
                AssetInfoItem(
                    burn_timestamp = "09-03-2023",
                    daily_interest_rate = 10,
                    id = "2ace1824-f14d-4bf6-b374-e467073ad90",
                    price = 50,
                    owners = Owners(
                        additionalProp1 = 0,
                        additionalProp2 = 0,
                        additionalProp3 = 0,
                    )
                ),
                AssetInfoItem(
                    burn_timestamp = "09-03-2023",
                    daily_interest_rate = 10,
                    id = "e9a1a742-d266-4d9d-81d9-933bb0a164bd",
                    price = 0,
                    owners = Owners(
                        additionalProp1 = 0,
                        additionalProp2 = 0,
                        additionalProp3 = 0,
                    )
                ),
            )
        )
    }

    override fun getDexOffers(): Flow<List<DexAsset>> = flow {
        emit(
            listOf(
                DexAsset(
                    burn_timestamp = "31-03-2023",
                    daily_interest_rate = 10,
                    id = "2ace1824-f14d-4bf6-b374-e467073ad90",
                    in_stock = 2,
                    price = 50,
                ),
                DexAsset(
                    burn_timestamp = "31-04-2023",
                    daily_interest_rate = 10,
                    id = "e9a1a742-d266-4d9d-81d9-933bb0a164bd",
                    in_stock = 10,
                    price = 1,
                ),
                DexAsset(
                    burn_timestamp = "31-03-2023",
                    daily_interest_rate = 2,
                    id = "2ab52h45-1843-1942-f4dv-2g186j12sab9",
                    in_stock = 1,
                    price = 150,
                ),
                DexAsset(
                    burn_timestamp = "31-06-2023",
                    daily_interest_rate = 2,
                    id = "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                    in_stock = 6,
                    price = 200,
                ),
                DexAsset(
                    burn_timestamp = "31-07-2023",
                    daily_interest_rate = 2,
                    id = "wv6zoqp7wnrv9p9djgfziyto4o2xwv98ggbi",
                    in_stock = 4,
                    price = 250,
                ),
                DexAsset(
                    burn_timestamp = "31-08-2023",
                    daily_interest_rate = 2,
                    id = "fub10ngvg7bx74ftuaxnqk71iubuq2nb3ms9",
                    in_stock = 3,
                    price = 350,
                ),
                DexAsset(
                    burn_timestamp = "31-09-2023",
                    daily_interest_rate = 2,
                    id = "cr3q64r04lbmkpvn80kum8h2dpxdyrf5liik",
                    in_stock = 1,
                    price = 400,
                ),
            )
        )
    }
}