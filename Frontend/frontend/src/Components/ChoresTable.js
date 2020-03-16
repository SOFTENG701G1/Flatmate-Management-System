import React from 'react';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import './chores.css';

class ChoresTable extends React.Component {
    constructor(props) {
        super(props);      
    }

    render() {
        
        //Currently inline styling for the table
        const headingStyle = {
            fontWeight: 'bold',
            color: 'lightblue',
            backgroundColor: '#1E1F26',
            fontSize: '16px'
        };

        const tableStyle = {
            backgroundColor: '#1E1F26',   
        };

        const cellStyle = {
            color: '#EEEEEE',
            fontSize: '14px'
        }

        return (
            <TableContainer component={Paper}>
              <Table style={tableStyle}> 
                <TableHead>
                  <TableRow>
                    <TableCell style={headingStyle}>Chore</TableCell>
                    <TableCell style={headingStyle} align="left">Assignee</TableCell>
                    <TableCell style={headingStyle} align="left">Date to complete by</TableCell>
                    <TableCell style={headingStyle} align="left">Complete status</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {this.props.chores.map(row => (
                    <TableRow key={row.chore}>
                      <TableCell style={cellStyle}> {row.chore}</TableCell>
                      <TableCell style={cellStyle} align="left">{row.assignee}</TableCell>
                      <TableCell style={cellStyle} align="left">{row.completeBy}</TableCell>
                      <TableCell style={cellStyle} align="left">
                          <input type="checkbox" checked={row.completed} ></input>
                          </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          );

    }
}

export default ChoresTable;