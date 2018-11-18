import ITradePair from "./ITradePair";
import IDictionary from "./IDictionary";
import IDepthMessage from "./IDepthMessage";
import * as _ from "lodash";

export default class ArbitragePair {
    private readonly _pair: ITradePair;
    private readonly _first: string;
    private readonly _second: string;
    private readonly _rate: IDictionary<number[]>;

    constructor(first: string, second: string, pair: ITradePair) {
        this._first = first;
        this._second = second;
        this._pair = pair;
        this._rate = {};
        this._rate[first] = [0, 0];
        this._rate[second] = [0, 0];
    }

    public get Symbol(): string {
        return this._pair.label;
    }

    public get RateBuy(): number {
        return this._rate[this._first][1]; //> this._rate[this._second][1] ? this._rate[this._second][1] : this._rate[this._first][1];
    }

    public get RateSell(): number {
        return this._rate[this._first][0];// > this._rate[this._second][0] ? this._rate[this._second][0] : this._rate[this._first][0];
    }

    public get Route(): string {
        if (this.RateBuy > this.RateSell) {
            return `${this._second}->${this._first}`;
        }

        return `${this._first}->${this._second}`;
    }

    public get Profit(): number {
        return _.floor((this.RateSell - this.RateBuy) / this.RateBuy * 100, 2);
    }

    public Refresh(depth: IDepthMessage): void {
        this._rate[depth.source] = [depth.orderBook.asks[0][0], depth.orderBook.bids[0][0]];
    }
}