import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import wsBinance from "./wsBinance";
import IOrderBook from "./models/IOrderBook";
import ITradePairsResponse from "./models/ITradePairsResponse";
import IDepthMessage from "./models/IDepthMessage";
import IWebSocket from "./IWebSocket";
import IDictionary from "./models/IDictionary";

export default class ArbitrageScanner {
    private readonly _connector: Connector = new Connector();
    private readonly _pairsMap: IDictionary<ITradePair> = {};
    private readonly _sockets: IWebSocket[] = [];
    private _pairs: ReadonlyArray<ITradePair> = [];
    private readonly _opportunities: IDictionary<IDictionary<ITradePair>> = {};

    constructor() {
        this._sockets.push(new wsBinance());
    }

    public get Pairs(): ReadonlyArray<ITradePair> {
        return this._pairs;
    }

    public async Start(): Promise<void> {
        await this.Initialize();
        let pairs: string[] = this._pairs.map(item => item.label);
        let local = this.DepthMessageHandler.bind(this);
        this._sockets.forEach(socket => {
            socket.SubscribeToDepth(5, pairs, local).Start();
        });
    }

    private DepthMessageHandler(depth: IDepthMessage): void {
        let pair: ITradePair = this._pairsMap[depth.pair];
        pair.rate = depth.orderBook.asks[0][0];

        let rateBuy: number = depth.orderBook.asks[0][0];
        let rateSell: number = depth.orderBook.asks[0][0];

    }

    private async Initialize(): Promise<void> {
        let response: ITradePairsResponse = await this._connector.GetTradePairsAsync({ apiName: "Binance" });
        response.pairs.forEach(pair => {
            this._pairsMap[pair.label] = pair;
        });
        this._pairs = response.pairs;
    }
}