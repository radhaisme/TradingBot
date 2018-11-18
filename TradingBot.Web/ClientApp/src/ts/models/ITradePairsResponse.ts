import ITradePair from "./ITradePair";

export default interface ITradePairsResponse {
    pairs: ReadonlyArray<ITradePair>
}