import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import wsBinance from "./wsBinance";
import IOrderBook from "./models/IOrderBook";
import ITradePairsResponse from "./models/ITradePairsResponse";
import IDepthMessage from "./models/IDepthMessage";
import IWebSocket from "./IWebSocket";
import IDictionary from "./models/IDictionary";
import TradePair from "./models/TradePair";

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
            // socket.SubscribeToDepth(5, pairs, local).Start();
        });
    }

    private DepthMessageHandler(depth: IDepthMessage): void {
        let pair: ITradePair = this._pairsMap[depth.pair];
        pair.rate = depth.orderBook.asks[0][0];

        let rateBuy: number = depth.orderBook.asks[0][0];
        let rateSell: number = depth.orderBook.asks[0][0];

    }

    private async Initialize(): Promise<void> {
        let pairs: IDictionary<ReadonlyArray<TradePair>> = await this.LoadAssets();

        let r: TradePair[] = this.Intersect(pairs["Binance"], pairs["Bitfinex"]);
        console.log(pairs.length);

        r.forEach(pair => {
            console.log(pair.Original.label);
        });





        // let set: Set<TradePair> = new Set();


        // let keys: string[] = Object.keys(pairs);

        // for (let i = 0; i < exchanges.length; i++) {
        //     for (let j = i + 1; j < exchanges.length; j++) {
        //         let commonPairs: ITradePair[] = this.Intersect(pairs[exchanges[i]], pairs[exchanges[j]]);
        //         let opportunity: IArbitrageOpportunity = {
        //             from: exchanges[i],
        //             to: exchanges[j],
        //             pairs: commonPairs
        //         };
        //     }
        // }
    }

    private async LoadAssets(): Promise<IDictionary<ReadonlyArray<TradePair>>> {
        let pairs: IDictionary<ReadonlyArray<TradePair>> = {};
        let exchanges: string[] = ["Binance", "Bitfinex"];
        exchanges.forEach(async ex => {
            let response: ITradePairsResponse = await this._connector.GetTradePairsAsync({ apiName: ex });
            pairs[ex] = response.pairs.map(pair => new TradePair(pair));
            // response.pairs.forEach(pair => {                
            //     if (!this._pairsMap.hasOwnProperty(pair.label)) {
            //         this._pairsMap[pair.label] = pair;
            //     }
            // });
        });

        return pairs;
    }

    private Intersect(first: ReadonlyArray<TradePair>, second: ReadonlyArray<TradePair>): TradePair[] {
        let longest: ReadonlyArray<TradePair>;
        let shortest: ReadonlyArray<TradePair>;

        if (first.length > second.length) {
            longest = first;
            shortest = second;
        }
        else {
            longest = second;
            shortest = first;
        }

        let temp: TradePair[] = [];

        longest.forEach((pair: TradePair) => {
            let result: TradePair = shortest.find((item: TradePair) => {
                return pair.Equals(item);
            });

            if (result) {
                temp.push(result);
            }
        });

        return temp;
    }
}

interface IArbitrageOpportunity {
    from: string;
    to: string;
    pairs: ITradePair[];
}