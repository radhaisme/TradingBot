import TradePair from "./TradePair";
import ITradePair from "./ITradePair";

export default interface ITradePairsResponse {
    pairs: ReadonlyArray<ITradePair>
}