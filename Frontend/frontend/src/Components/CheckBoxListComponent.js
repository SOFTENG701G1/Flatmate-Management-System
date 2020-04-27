import React, {Component} from 'react';
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
                        <CheckBoxList listItems={this.props.listItems} 
                        onListItemChanged={this.props.onListItemChanged}
                        isChecked={this.props.isChecked}
                        relatedId={this.props.relatedId}/>
                    </div>
                </Row>
            </Container>
        );
    }
}

function CheckBoxList(props) {
    var onListItemChanged = props.onListItemChanged.bind();
    // Map each chore item to a FormControlLabel as it allows checkboxes to have text
    const checkBoxes = props.listItems.map((item, index) =>
        <Row>
            <FormControlLabel
                control={
                   <Checkbox onChange={() => {onListItemChanged(props.relatedId[index])}} 
                    defaultChecked={props.isChecked[index]}/>
                }
                label={item}   
            />
        </Row>
    );

    return <>{checkBoxes}</>;
  }