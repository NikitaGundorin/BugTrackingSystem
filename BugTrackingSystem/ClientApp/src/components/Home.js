import React, { Component } from 'react';
import { Table, Button } from 'reactstrap';
import { bugService } from '../services/BugService';
import { history } from '../App'
import "./style.css";


const Row = ({ id, creationDate, shortDescription, importance, priority, status, userName }) => (
  <tr style={{varticalAlign : 'center'}}>
    <td>{id}</td>
    <td>{creationDate}</td>
    <td>{shortDescription}</td>
    <td>{importance}</td>
    <td>{priority}</td>
    <td>{userName}</td>
    <td>{status}</td>
    <td><Button outline block color="primary" size="sm" style={{lineHeight: "1rem"}} onClick={() => {openBug(id)}}>Edit</Button></td>
  </tr>
);

function openBug(id) {
  history.push('/bug/' + id);
};

export class Home extends Component {
  static displayName = Home.name;
  
  constructor(props) {
    super(props);
    this.state = {
      data: null,
    };
  }
  

  componentDidMount() {
    bugService.getBugs().then(data => this.setState({ data }));
  }

  render() {
    const rows = this.state.data ? this.state.data.map((rowData) => <Row {...rowData} key={rowData.id} />) : null;
    return (
      <div>
        <h1>All bugs</h1>
        {rows &&
          <Table hover>
            <thead>
              <tr>
                <th>ID</th>
                <th>Creation Date</th>
                <th>Short Description</th>
                <th>Importance</th>
                <th>Priority</th>
                <th>User</th>
                <th>Status</th>
                <th>Edit</th>
              </tr>
            </thead>
            <tbody>
              {rows}
            </tbody>
          </Table>
        }
      </div>
    );
  }
}
