import Connector from "./Connector";
import ITradePair from "./models/ITradePair";
import wsBinance from "./wsBinance";
import ITradePairsResponse from "./models/ITradePairsResponse";
import IDepthMessage from "./models/IDepthMessage";
import IWebSocket from "./IWebSocket";
import IDictionary from "./models/IDictionary";
import IArbitrageOpportunity from "./models/IArbitrageOpportunity";
import ArbitragePair from "./models/ArbitragePair";
import * as _ from "lodash";

export default class ArbitrageScanner {
    private readonly _connector: Connector = new Connector();
    private readonly _sockets: IWebSocket[] = [];
    private readonly _exchanges: string[] = ["Binance", "Bitfinex"];
    private readonly _arbitragePairs: IDictionary<IDictionary<ArbitragePair>> = {};
    private _pairs: ReadonlyArray<ArbitragePair> = [];

    constructor() {
        this._sockets.push(new wsBinance());
    }

    public get Pairs(): ReadonlyArray<ArbitragePair> {
        return this._pairs;
    }

    public async Start(): Promise<void> {
        await this.Initialize();
        let pairs: string[] = this._pairs.map(item => item.Symbol);
        let local = this.DepthMessageHandler.bind(this);
        this._sockets.forEach(socket => {
            socket.SubscribeToDepth(5, pairs, local).Start();
        });
    }

    private DepthMessageHandler(depth: IDepthMessage): void {
        this._arbitragePairs[depth.source][depth.pair].Refresh(depth);
    }

    private async Initialize(): Promise<void> {
        const pairs: IDictionary<ReadonlyArray<ITradePair>> = await this.LoadAssets();
        const opportunities: IArbitrageOpportunity[] = [];

        for (let i = 0; i < this._exchanges.length; i++) {
            for (let j = i + 1; j < this._exchanges.length; j++) {
                let opportunity: IArbitrageOpportunity = {
                    first: this._exchanges[i],
                    second: this._exchanges[j],
                    pairs: _.intersectionWith(pairs[this._exchanges[i]], pairs[this._exchanges[j]], _.isEqual)
                };
                opportunities.push(opportunity);
            }
        }

        opportunities.forEach(op => {
            op.pairs.forEach(pair => {
                let item: ArbitragePair = new ArbitragePair(op.first, op.second, pair);

                if (!_.has(this._arbitragePairs, [op.first, pair.label])) {
                    _.set(this._arbitragePairs, `${op.first}.${pair.label}`, item);
                }

                if (!_.has(this._arbitragePairs, [op.second, pair.label])) {
                    _.set(this._arbitragePairs, `${op.second}.${pair.label}`, item);
                }
            });
        });
        this._pairs = _.uniq(_.flatMap(_.values(this._arbitragePairs).map(item => _.values(item))));
    }

    private async LoadAssets(): Promise<IDictionary<ReadonlyArray<ITradePair>>> {
        const pairs: IDictionary<ReadonlyArray<ITradePair>> = {};

        for (let ex of this._exchanges) {
            let response: ITradePairsResponse = await this._connector.GetTradePairsAsync({ apiName: ex });
            pairs[ex] = response.pairs;
        }

        return pairs;
    }
}