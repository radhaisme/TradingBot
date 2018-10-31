import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import ITradePairsResponse from "./models/ITradePairsResponse";

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

        // let symbol: string = "btcusdt";
        // let depth: number = 5;
        // let tradeStream: string = `${symbol}@depth${depth}`;
        // let ws: WebSocket = new WebSocket(`wss://stream.binance.com:9443/stream?streams=${tradeStream}`);
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