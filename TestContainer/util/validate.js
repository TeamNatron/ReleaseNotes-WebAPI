const dateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{5}$/;

/* Checks if incoming date matches the set regex for correct dates */
const correctDateFormat = inputDate => {
  if (dateFormat.test(inputDate)) {
    return true;
  }

  return false;
};

module.exports = correctDateFormat;
