import React, {Component, useState} from 'react';
import './ChoresUpdateButton.css';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import MultiSelect from "react-multi-select-component";
import Select from 'react-select';

function ChoresUpdateButton(props){
  const [popupState, setPopupState] = React.useState(false);

  const [selected, setSelected] = useState([]);


  const handleOpenPopup = () => {
    setPopupState(true);
  };
  const handleClosePopup = () => {
    setPopupState(false);
  };
  
  const options = [
    { value: 'yin', label: 'yin' },
    { value: 'teresa', label: 'teresa' },
    { value: 'bryan', label: 'bryan' }
  ]
  return (
    <div>
      <Button style={{"background-color":"#1F5673", "color": "white"}} onClick={handleOpenPopup}>
        Add Chore
      </Button>
      <Dialog open={popupState} onClose={handleClosePopup} aria-labelledby="form-dialog-title">
        <DialogTitle id="form-dialog-title">Add Chore</DialogTitle>
        {/*To do: Change highlight colour from*/}
        <DialogContent > 
          <DialogContentText className="chores_description">
            Use following form to assign people or yourself chores. You can see your assigned chores
            on the dashboard
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            id="title"
            label="Title"
            defaultValue={props.title}
            fullWidth
          />
          <div className="chores_assign">
            <InputLabel  id="demo-simple-select-label" style={{"margin-bottom":"10px"}}>Assigned Flat Members</InputLabel>
            <Select
              defaultValue={[options[1], options[2]]}
              isMulti
              name="colors"
              options={options}
              className="basic-multi-select"
              classNamePrefix="select"
            />
          </div>
          <TextField
            autoFocus
            margin="dense"
            id="description"
            label="Description"
            defaultValue={props.description}
            fullWidth
            multiline
            rows="4"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClosePopup} color="primary">
            Cancel
          </Button>
          <Button onClick={handleClosePopup} color="primary">
            Add
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default ChoresUpdateButton;