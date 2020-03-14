import React, {Component, Text} from 'react';
import '../App.css';
import Checkbox from '@material-ui/core/Checkbox';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/row';
import FormControlLabel from '@material-ui/core/FormControlLabel';

export default class ChoreListComponent extends Component {
    render () {
        return (
            <Container className="ChoreListContainer">
                <Row className="ChoreListHeader">Chores</Row>
                <Row>
                    <div className = "ChoreList">
                        <ChoreList/>
                    </div>
                </Row>
            </Container>
        );
    }
}

function ChoreList() {
    // Following values are placeholders until chore functionality is completed
    const chores = ["Wash bathroom", "Buy groceries", "Do laundry", "Cook dinner"];

    // Map each chore item to a FormControlLabel as it allows checkboxes to have text
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