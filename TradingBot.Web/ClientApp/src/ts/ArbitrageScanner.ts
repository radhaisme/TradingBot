import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import ITradePairsResponse from "./models/ITradePairsResponse";
import wsBinance from "./wsBinance";
import IOrderBook from "./models/IOrderBook";

export default class ArbitrageScanner {
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

        let ws = new wsBinance();
        ws.SubscribeToDepth(5, ["BTCUSDT", "ETHBTC"], (depth: IOrderBook): void => {
            console.log(depth);
        }).Start();
        // ws.Subscribe();

        // let symbol: string = "btcusdt";
        // let depth: number = 5;
        // let tradeStream: string = `${symbol}@depth${depth}`;
        // let wsUrl: string = `wss://stream.binance.com:9443/stream?streams=${tradeStream}`;
        // console.log(wsUrl);
        // let ws: WebSocket = new WebSocket(wsUrl);
        // ws.onopen = () => {
        //     console.log("Opened");
        // };
        // ws.onmessage = (ev: MessageEvent) => {
        //     console.log(ev);
        // };
    }

    /**
     * Test method
     */
    public async Start(): Promise<ReadonlyArray<ITradePair>> {
        let connector = new Connector();

        return (await connector.GetTradePairsAsync({ apiName: "Binance" })).pairs;
    }
}