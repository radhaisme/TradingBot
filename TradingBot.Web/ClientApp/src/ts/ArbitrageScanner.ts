import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import ITradePairsResponse from "./models/ITradePairsResponse";

export default class ArbitrageScanner {
    constructor() {
        let connector = new Connector();
        let pairs: { [key: string]: ITradePair } = {};
        let arr: ITradePair[] = [];
        let exchanges: string[] = ["Binance", "Bitfinex"];
        exchanges.forEach((item: string) => {
            let promises: Promise<ITradePairsResponse>[] = [];
            promises.push(connector.GetTradePairsAsync({ apiName: item }));
            Promise.all<ITradePairsResponse>(promises).then(response => {
                response.forEach(item => {
                    item.pairs.forEach(pair => {
                        pairs[pair.label] = pair;
                    });
                });
                Object.keys(pairs).forEach(key => {
                    arr.push(pairs[key]);
                });
            });
        });
    }

    /**
     * Test method
     */
    public sayHello(): void {
        console.log("hello");
    }
}