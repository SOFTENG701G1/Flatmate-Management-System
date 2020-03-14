import React, {Component, Text} from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import FormControlLabel from '@material-ui/core/FormControlLabel';

export default class ChoreListComponent extends Component {
    constructor () {
        super();
    }

    render () {
        return (
            <div className = "ChoreListComponent">
                <ChoreList/>
            </div>
      )
  }
}

function ChoreList() {
    const chores = ["Wash bathroom", "Buy groceries", "Do laundry", "Cook dinner"];

    const checkBoxes = chores.map((chore) =>
        <FormControlLabel
            control={
                <Checkbox/>
            }
            label={chore}   
        />
    );

    return <>{checkBoxes}</>;
  }