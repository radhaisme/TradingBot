import ITradePair from "./ITradePair";

export default class ArbitragePair {
    private readonly _pair: ITradePair;
    private readonly _from: string;
    private readonly _to: string;
    private _rateBuy: number;
    private _rateSell: number;

    constructor(from: string, to: string, pair: ITradePair) {
        this._from = from;
        this._to = to;
        this._pair = pair;
    }

    public get RateBuy(): number {
        return 1;
    }

    public get RateSell(): number {
        return 0;
    }

    public get Symbol(): string {
        return this._pair.label;
    }

    public get Route(): string {
        if (this.RateBuy > this.RateSell) {
            return `${this._to}->${this._from}`;
        }

        return `${this._from}->${this._to}`;
    }

    public get Rate(): string {
        return `${this._rateBuy}->${this._rateSell}`;
    }
}