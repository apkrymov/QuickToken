package org.quicktoken.android.utils

import org.quicktoken.android.utils.SmartContractAddresses.DEX_CONTRACT
import org.quicktoken.android.utils.SmartContractAddresses.QTKA_CONTRACT
import org.web3j.abi.FunctionEncoder
import org.web3j.abi.TypeReference
import org.web3j.abi.datatypes.Address
import org.web3j.abi.datatypes.Bool
import org.web3j.abi.datatypes.Function
import org.web3j.abi.datatypes.generated.Uint256
import java.math.BigInteger

object Web3Utils {

    fun encodeApprove(assetId: BigInteger): String {
        val function = Function(
            "approve",  // function we're calling
            listOf(
                Address(DEX_CONTRACT),
                Uint256(assetId)
            ),  // Parameters to pass as Solidity Types
            listOf(object : TypeReference<Bool?>() {})
        )
        return FunctionEncoder.encode(function)
    }

    fun encodeBuy(assetId: BigInteger): String {
        val function = Function(
            "buy",  // function we're calling
            listOf(
                Uint256(assetId)
            ),  // Parameters to pass as Solidity Types
            listOf(object : TypeReference<Bool?>() {})
        )
        return FunctionEncoder.encode(function)
    }

    fun encodeSell(assetId: BigInteger): String {
        val function = Function(
            "sell",  // function we're calling
            listOf(
                Uint256(assetId)
            ),  // Parameters to pass as Solidity Types
            listOf(object : TypeReference<Bool?>() {})
        )
        return FunctionEncoder.encode(function)
    }
}