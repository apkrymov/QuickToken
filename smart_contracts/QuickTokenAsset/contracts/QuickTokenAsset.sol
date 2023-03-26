// SPDX-License-Identifier: MIT
pragma solidity >=0.8.0 <0.9.0;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Burnable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";

uint256 constant secondsInDay = 86400;

uint8 constant interestRateDecimals = 9;
uint256 constant interestRateMultiplier = 100 * 10 ** interestRateDecimals;

contract QuickTokenAsset is ERC721, ERC721Enumerable, ERC721Burnable, Ownable {
    struct Metadata {
        uint256 ipoSerial;
        uint256 ipoPrice;
        uint256 ipoTimestamp;
        uint256 dailyInterestRate;
        uint256 burnTimestamp;
    }

    // Mapping from token id to token data
    mapping(uint256 => Metadata) private _metadataMapping;

    constructor() ERC721("QuickTokenAsset", "QTKA") {}

    function serialMint(address to, uint256[] memory tokenIds, Metadata memory inputMetadata) external onlyOwner {
        for (uint256 i = 0; i < tokenIds.length; i++){
            _metadataMapping[tokenIds[i]] = inputMetadata;
            _safeMint(to, tokenIds[i]);
        }
    }

    function price(uint256 tokenId) external view returns (uint256) {
        _requireMinted(tokenId);

        uint256 ipoTimestamp = _metadataMapping[tokenId].ipoTimestamp;
        uint256 daysSinceIpo =  (block.timestamp - ipoTimestamp) / secondsInDay;

        uint256 ipoPrice = _metadataMapping[tokenId].ipoPrice;
        uint256 dailyInterestRate = _metadataMapping[tokenId].dailyInterestRate;

        return (ipoPrice * ((dailyInterestRate + interestRateMultiplier) ** daysSinceIpo)) / (interestRateMultiplier) ** daysSinceIpo;
    }

    function metadata(uint256 tokenId) external view returns (Metadata memory) {
        _requireMinted(tokenId);
        return _metadataMapping[tokenId];
    }

    function isBurnable (uint256 tokenId) public view returns (bool) {
        _requireMinted(tokenId);
        return block.timestamp > _metadataMapping[tokenId].burnTimestamp;
    }

    function burn(uint256 tokenId) public override {
        require(_isApprovedOrOwner(_msgSender(), tokenId), "ERC721: caller is not token owner or approved");
        require(isBurnable(tokenId), "QTKA: burn date is not yet reached");
        _burn(tokenId);
        delete _metadataMapping[tokenId];
    }

    // The following functions are overrides required by Solidity.

    function _beforeTokenTransfer(address from, address to, uint256 tokenId, uint256 batchSize)
        internal
        override(ERC721, ERC721Enumerable)
    {
        super._beforeTokenTransfer(from, to, tokenId, batchSize);
    }

    function supportsInterface(bytes4 interfaceId)
        public
        view
        override(ERC721, ERC721Enumerable)
        returns (bool)
    {
        return super.supportsInterface(interfaceId);
    }
}
