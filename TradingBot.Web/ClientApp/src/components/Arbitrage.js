import React, { Component } from 'react';
import ArbitrageScanner from "../js/ArbitrageScanner";

export class Arbitrage extends Component {
  displayName = Arbitrage.name;

  constructor(props) {
    super(props);
    this.state = { pairs: [], loading: true };
    var scanner = new ArbitrageScanner();
    scanner.Start().then(pairs => {
      this.setState({ pairs: pairs, loading: false });
    });
  }

  static renderTable(pairs) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Pair</th>
            <th>Exchange (buy->sell)</th>
            <th>Rate (buy->sell)</th>
            <th>Spread (%)</th>
            <th>Profit (%)</th>
          </tr>
        </thead>
        <tbody>
          {pairs.map(pair =>
            <tr key={pair.label}>
              <td>{pair.label}</td>
              <td>{"Binance->Bitfinex"}</td>
              <td>{"0->0.5"}</td>
              <td>{0.1}</td>
              <td>+{10}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Arbitrage.renderTable(this.state.pairs);

    return (
      <div>
        <h1>Arbitrage opportunities</h1>
        <p>This component demonstrates searching arbitrage opportunities.</p>
        {contents}
      </div>
    );
  }
}