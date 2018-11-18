import ITradePair from "./ITradePair";

export default interface IArbitrageOpportunity {
    readonly first: string;
    readonly second: string;
    readonly pairs: ITradePair[];
}