import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import wsBinance from "./wsBinance";
import IOrderBook from "./models/IOrderBook";
import ITradePairsResponse from "./models/ITradePairsResponse";

export default class ArbitrageScanner {
    private readonly _connector: Connector = new Connector();
    private _pairs: ReadonlyArray<ITradePair> = [];

    constructor() {
        // let pairs: { [key: string]: ITradePair } = {};
        // let exchanges: string[] = ["Binance", "Bitfinex"];
        // exchanges.forEach((item: string) => {
        //     let promises: Promise<ITradePairsResponse>[] = [];
        //     promises.push(connector.GetTradePairsAsync({ apiName: item }));
        //     Promise.all<ITradePairsResponse>(promises).then(response => {
        //         response.forEach(item => {
        //             item.pairs.forEach(pair => {
        //                 pairs[pair.label] = pair;
        //             });
        //         });
        //     });
        // });        
    }

    public get Pairs(): ReadonlyArray<ITradePair> {
        return this._pairs;
    }

    public async Start(): Promise<boolean> {
        // let ws = new wsOkex();
        // ws.SubscribeToDepth(5, ["btc_usdt"], (depth: IOrderBook): void => { });
        // ws.Start();

        // return (await this._connector.GetTradePairsAsync({ apiName: "Binance" })).pairs;

        let response: ITradePairsResponse = await this._connector.GetTradePairsAsync({ apiName: "Binance" });
        this._pairs = response.pairs;
        let ws = new wsBinance();
        ws.SubscribeToDepth(5, ["BTCUSDT"], (depth: IOrderBook): void => {
            let pair: ITradePair = this._pairs.find((item: ITradePair): boolean => { return item.label === "BTC/USDT"; });
            pair.rate = depth.asks[0][0];
        }).Start();

        return true;
    }
}