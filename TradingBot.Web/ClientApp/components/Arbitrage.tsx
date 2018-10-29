import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import Connector from "../ts/Connector";
import ITradePairsResponse from "../ts/models/ITradePairsResponse";
import ITradePair from '../ts/models/ITradePair';

interface IArbitrageState {
    pairs: ITradePair[];
    loading: boolean;
}

export class Arbitrage extends React.Component<RouteComponentProps<{}>, IArbitrageState> {
    constructor() {
        super();
        this.state = { pairs: [], loading: true };
        var connector = new Connector();
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
                this.setState({ pairs: arr, loading: false });
            });
        });
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Arbitrage.renderPairsTable(this.state.pairs);

        return <div>
            <h1>Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>;
    }

    private static renderPairsTable(pairs: ITradePair[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>Pair</th>
                    {/* <th>Exchange (Buy)</th>
                    <th>Exchange (Sell)</th>
                    <th>Future profit - (%)</th> */}
                </tr>
            </thead>
            <tbody>
                {pairs.map(item =>
                    <tr key={item.label}>
                        <td>{item.label}</td>
                        {/* <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td> */}
                    </tr>
                )}
            </tbody>
        </table>;
    }
}

interface IArbitragePair {
    pair: string;
}