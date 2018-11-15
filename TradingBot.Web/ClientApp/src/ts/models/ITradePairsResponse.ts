import TradePair from "./TradePair";

export default interface ITradePairsResponse {
    pairs: ReadonlyArray<TradePair>
}