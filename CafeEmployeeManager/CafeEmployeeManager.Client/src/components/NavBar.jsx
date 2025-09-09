import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link } from 'react-router-dom';

const NavBar = () => (
  <AppBar position="static" color="primary" sx={{ mb: 4 }}>
    <Toolbar>
      <Typography variant="h6" sx={{ flexGrow: 1 }}>
        Cafe Employee Manager
      </Typography>
      <Box>
        <Button color="inherit" component={Link} to="/cafes">Cafe</Button>
        <Button color="inherit" component={Link} to="/employees">Employee</Button>
      </Box>
    </Toolbar>
  </AppBar>
);

export default NavBar;