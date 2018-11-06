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

  static renderTable(opportunities) {
    return (
      <table className='table'>
        <thead>
          <tr>
            <th>Pair</th>
            <th>Exchange (buy->sell)</th>
            <th>Rate (buy->sell)</th>
            <th>Spread (%)</th>
            <th>Profit (%)</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {opportunities.map(opportunity =>
            <tr key={opportunity.label}>
              <td>{opportunity.label}</td>
              <td>{"Binance->Bitfinex"}</td>
              <td>{opportunity.rate}</td>
              <td>{0.1}</td>
              <td>+{10}</td>
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