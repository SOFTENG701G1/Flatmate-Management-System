import React, {Component, Text} from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import FormControlLabel from '@material-ui/core/FormControlLabel';

export default class ChoreListComponent extends Component {
    constructor () {
        super();
    }

    render () {
        return (
            <Container className = "ChoreListComponent">
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