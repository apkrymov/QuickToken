// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0 <0.9.0;

import "@openzeppelin/contracts/token/ERC721/IERC721Receiver.sol";
import "@openzeppelin/contracts/token/ERC721/IERC721.sol";
import "@openzeppelin/contracts/token/ERC20/IERC20.sol";

interface IQuickTokenCurrency is IERC20 {
}

interface IQuickTokenAsset is IERC721 {
    function price(uint256 tokenId) external view returns (uint256);
    function isBurnable (uint256 tokenId)  external view returns (bool);
    function burn(uint256 tokenId) external;
}

contract QuickTokenDEX is IERC721Receiver {
    IQuickTokenCurrency private _currencyContract;
    IQuickTokenAsset private _assetContract;

    constructor(address _currencyContractAddress, address _assetContractAddress) {
        _currencyContract = IQuickTokenCurrency(_currencyContractAddress);
        _assetContract = IQuickTokenAsset(_assetContractAddress);
    }

    function buy(uint256 assetId) external {
        uint256 assetPrice = _assetContract.price(assetId);
        _assetContract.safeTransferFrom(address(this), msg.sender, assetId);
        _currencyContract.transferFrom(msg.sender, address(this), assetPrice);
    }

    function sell(uint256 assetId) external {
        uint256 assetPrice = _assetContract.price(assetId);
        _assetContract.safeTransferFrom(msg.sender, address(this), assetId);
        _currencyContract.transfer(msg.sender, assetPrice);
        if(_assetContract.isBurnable(assetId)){
            _assetContract.burn(assetId);
        }
    }

    function onERC721Received(address, address, uint256, bytes calldata) external override pure returns (bytes4) {
        return IERC721Receiver.onERC721Received.selector;
    }
}
