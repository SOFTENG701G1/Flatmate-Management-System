import React, {Component, Text} from 'react';
import '../App.css';
import Checkbox from '@material-ui/core/Checkbox';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/row';
import FormControlLabel from '@material-ui/core/FormControlLabel';

export default class ChoreListComponent extends Component {
    render () {
        return (
            <Container className = "ChoreListComponent">
                <Row style={{'font-weight':'bold'}}>Chores</Row>
                <ChoreList/>
            </Container>
        );
    }
}

function ChoreList() {
    const chores = ["Wash bathroom", "Buy groceries", "Do laundry", "Cook dinner"];

    const checkBoxes = chores.map((chore) =>
        <Row>
            <FormControlLabel
                control={
                    <Checkbox/>
                }
                label={chore}   
            />
        </Row>
    );

    return <>{checkBoxes}</>;
  }