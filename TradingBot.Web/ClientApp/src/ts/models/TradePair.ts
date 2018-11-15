import ITradePair from "./ITradePair";

export default class TradePair {
    private readonly _pair: ITradePair;

    constructor(pair: ITradePair) {
        this._pair = pair;
    }

    public get Original(): ITradePair {
        return this._pair;
    }

    public Equals(pair: TradePair): boolean {
        if (!pair) {
            return false;
        }

        return this.Original.label === pair.Original.label;
    }

    public valueOf(): string {
        return this._pair.label;
    }

    public toString(): string {
        return this._pair.label;
    }
}