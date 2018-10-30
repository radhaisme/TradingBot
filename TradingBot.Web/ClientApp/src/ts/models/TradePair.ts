export default class TradePair {
    private readonly _label: string;

    constructor(label: string) {
        this._label = label;
    }

    public get label(): string {
        return this._label;
    }

    public valueOf(): string {
        return this.label;
    }

    public toString(): string {
        return this.label;
    }
}