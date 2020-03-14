import React, {Component, Text} from 'react';
import '../App.css';
import Checkbox from '@material-ui/core/Checkbox';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/row';
import FormControlLabel from '@material-ui/core/FormControlLabel';

export default class CheckBoxListComponent extends Component {
    render () {
        return (
            <Container className="CheckBoxListContainer">
                <Row className="CheckBoxListHeader">{this.props.title}</Row>
                <Row>
                    <div className = "CheckBoxList">
                        <CheckBoxList listItems={this.props.listItems}/>
                    </div>
                </Row>
            </Container>
        );
    }
}

function CheckBoxList(props) {
    // Map each chore item to a FormControlLabel as it allows checkboxes to have text
    const checkBoxes = props.listItems.map((item) =>
        <Row>
            <FormControlLabel
                control={
                    <Checkbox/>
                }
                label={item}   
            />
        </Row>
    );

    return <>{checkBoxes}</>;
  }