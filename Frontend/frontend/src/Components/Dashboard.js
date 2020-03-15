import React, {Component} from 'react';
import '../App.css';
import CheckBoxListComponent from './CheckBoxListComponent';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/row';
import Col from 'react-bootstrap/col';

export default class Dashboard extends Component {
    render () {
        return (
            <Container>
                <Row>
                {/* inline styling is a workaround until further components are added to pad out spacing*/}
                    <Col style={{"width":"30%", "margin-left":"300px"}}>
                    <Row>
                        <CheckBoxListComponent 
                            title="Chores"
                            listItems={["Wash bathroom", "Buy groceries", "Do laundry", "Cook dinner"]}
                        />
                    </Row>
                    <hr className="LineDivider"/>
                    <Row>
                        <CheckBoxListComponent 
                            title="Shopping List"
                            listItems={["Eggs", "Milk", "TP", "Cereal", "Pasta", "Sweetcorn"]}
                        />
                    </Row>
                    </Col>
                </Row>
            </Container>
        );
    }
}