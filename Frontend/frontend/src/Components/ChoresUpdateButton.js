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
import MultiSelect from "react-multi-select-component";
import Select from 'react-select';

/** This sets up the component that is a button which the user can click on 
 * which opens up a popup where the users can fill in chores information
 * The add functionality currently doesn't work and at the moment it just mimics it.
 * This component is also meant to for editing existing chores, currently you can 
 * pass in details to fill in the popup for editing, but the application currently doesn't
 * have the edit feature. I will be useful in the future
 * To do: Implement the adding chores functionality
 */
function ChoresUpdateButton(props){
  const [popupState, setPopupState] = React.useState(false);
  const handleOpenPopup = () => {
    setPopupState(true);
  };
  const handleClosePopup = () => {
    setPopupState(false);
  };
  
  /** Fixed options for the users you can add, 
   * To do: make this dynamic and depend on actual members
   */
  const options = [
    { value: 'yin', label: 'Yin' },
    { value: 'teresa', label: 'Teresa' },
    { value: 'bryan', label: 'Bryan' }
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
          {/** Allows to add a title, can pass in title description using props and set it as default value*/}
          <TextField
            autoFocus
            margin="dense"
            id="title"
            label="Title"
            defaultValue={props.title}
            fullWidth
          />
          {/** Allows members to be assigned to chores, can pass in members using props and set it as default value
           * The passed in memeber should be an array of objects in format: { value: (username all lower case), label: (username)}
           * e.g. { value: 'yin', label: 'Yin' }
           * To do: Improve how members are passed in
           */}
          <div className="chores_assign">
            <InputLabel  id="demo-simple-select-label" style={{"margin-bottom":"10px"}}>Assigned Flat Members</InputLabel>
            <Select
              defaultValue={props.members}
              isMulti
              name="colors"
              options={options}
              className="basic-multi-select"
              classNamePrefix="select"
            />
          </div>
          {/** Allows to add a description, can pass in description using props and set it as default value*/}
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