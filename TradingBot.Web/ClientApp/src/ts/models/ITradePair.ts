export default interface ITradePair {
    readonly label: string;
    readonly baseAsset: string;
    readonly quoteAsset: string;
    rate: number;
}