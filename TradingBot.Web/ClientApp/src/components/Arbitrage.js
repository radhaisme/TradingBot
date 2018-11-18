import React, { Component } from 'react';
import ArbitrageScanner from "../js/ArbitrageScanner";

export class Arbitrage extends Component {
  displayName = Arbitrage.name;
  _scanner = new ArbitrageScanner();

  constructor(props) {
    super(props);
    this.state = { pairs: [], loading: true };
    (async () => {
      await this._scanner.Start();
      setInterval(() => {
        this.setState({ pairs: this._scanner.Pairs, loading: false });
      }, 3000);
    })();
  }

  static renderTable(arbitragePairs) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Pair</th>
            <th>Exchange (buy->sell)</th>
            <th>Rate (buy->sell)</th>
            <th>Profit (%)</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {arbitragePairs.map(arbitragePair =>
            <tr key={arbitragePair.Symbol}>
              <td>{arbitragePair.Symbol}</td>
              <td>{arbitragePair.Route}</td>
              <td>{`${arbitragePair.RateBuy}->${arbitragePair.RateSell}`}</td>
              <td>+{arbitragePair.Profit}</td>
              <td><button onClick={() => { alert("СКОРЕЕЕ!!!!"); }}>Take profit!!!</button></td>
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