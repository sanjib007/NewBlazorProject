// IPv6 address 
var networkAddress = new Array(0, 0, 0, 0, 0, 0, 0, 0),
	ADDRESS_BLOCKS = new Array();

var TOTAL_BITS = 128,
	MASK_BITS = 0,
	NETWORK_BITS = 0,
	DEFAULT_NETWORK_BITS = 8,
	TOTAL_NETWORK_BITS = 64,
	SUBNET_BITS = 0,
	HOST_BITS = 64,
	TOTAL_HOST_BITS = 64,
	WORD = 16,
	TOTAL_WORD_COUNT = 8,
	TOTAL_COLON_COUNT = 7,
	HEX_BLOCK_DIGITS = 4,
	DEFAULT_ADDRESS_BLOCK = 'block1',  //No I18N
	SUBNET_LIST_START_INDEX = 0,
	SUBNET_LIST_END_INDEX = 16,
	MAX_SIZE_OF_SUBNET = 1024;

// variables for testing
var sampleIPv6Address = 'FE80:0000:0000:0000:0202:B3FF:FE1E:8329';  //No I18N
var truncatedIPv6Address = 'fd30::1:ff4e:3e:9:e';  //No I18N

function setTotalBits(value) {
	TOTAL_BITS = value;
}

function getTotalBits() {
	return TOTAL_BITS;
}

function setMaskBits(value) {
	MASK_BITS = value;
}

function getMaskBits() {
	return MASK_BITS;
}

function setSubnetBits(value) {
	SUBNET_BITS = value;
}

function getSubnetBits() {
	return SUBNET_BITS;
}

function setHostBits(value) {
	HOST_BITS = value;
}

function getHostBits() {
	return HOST_BITS;
}

function setNetworkBits(value) {
	NETWORK_BITS = value;
}

function getNetworkBits() {
	return NETWORK_BITS;
}

function setNetworkAddress(networkAddressArray) {
	networkAddress = networkAddressArray;
}

function getNetworkAddress() {
	return networkAddress;
}

function setSubnetListStartIndex(value) {
	SUBNET_LIST_START_INDEX = value;
}

function getSubnetListStartIndex() {
	return SUBNET_LIST_START_INDEX;
}

function setSubnetListEndIndex(value) {
	SUBNET_LIST_END_INDEX = value;
}

function getSubnetListEndIndex() {
	return SUBNET_LIST_END_INDEX;
}

function condenseHexAddress(formattedHexAddress, leadingZeroes) {
	if (formattedHexAddress.indexOf(":0000:0000:0000:0000:0000:0000:0000") > 0) {
		formattedHexAddress = formattedHexAddress.replace(':0000:0000:0000:0000:0000:0000:0000', '::');
	} else if (formattedHexAddress.indexOf(":0000:0000:0000:0000:0000:0000") > 0) {
		formattedHexAddress = formattedHexAddress.replace(':0000:0000:0000:0000:0000:0000', '::');
	} else if (formattedHexAddress.indexOf(":0000:0000:0000:0000:0000") > 0) {
		formattedHexAddress = formattedHexAddress.replace(':0000:0000:0000:0000:0000', '::');
	} else if (formattedHexAddress.indexOf(":0000:0000:0000:0000") > 0) {
		formattedHexAddress = formattedHexAddress.replace(':0000:0000:0000:0000', '::');
	} else if (formattedHexAddress.indexOf(":0000:0000:0000") > 0) {
		formattedHexAddress = formattedHexAddress.replace(':0000:0000:0000', '::');
	} else if (formattedHexAddress.indexOf(":0000:0000") > 0) {
		formattedHexAddress = formattedHexAddress.replace(':0000:0000', '::');
	}
	formattedHexAddress = formattedHexAddress.replace(':::', '::');
	if (leadingZeroes) {
		formattedHexAddress = formattedHexAddress.replace(/:0000/g, '==========');
		formattedHexAddress = formattedHexAddress.replace(/:000/g, ':');
		formattedHexAddress = formattedHexAddress.replace(/:00/g, ':');
		formattedHexAddress = formattedHexAddress.replace(/:0/g, ':');
		formattedHexAddress = formattedHexAddress.replace(/==========/g, ':0000');
	}
	return formattedHexAddress;
}

//Converts IPv6 in the form 'fd30::1:ff4e:3e:9:e' to 'fd30:0000:0000:0001:ff4e:003e:0009:000e'
function formatHexAddress(rawInputAddress) {
	//rawInputAddress = 'fe80::';
	//rawInputAddress = 'FE80:0000:0000:0000:0202:B3FF:FE1E:8329';
	//rawInputAddress = 'fdf4:af5d:8d13:7664::';
	//console.log('RawInputAddress : '+rawInputAddress+', Type : '+typeof(rawInputAddress));	
	var toReturnArray = new Array(),
		inputAddressArray = 0,
		totalNoOfColons = 0,
		zeroesToAppendStr = '';
	if (typeof (rawInputAddress) == 'object') {
		toReturnArray = rawInputAddress;
	} else {
		var noOfColon = rawInputAddress.split(":").length;
		var indexOfDoubleColon = rawInputAddress.indexOf("::");
		//console.log('noOfColon : '+noOfColon+', indexOfDoubleColon : '+indexOfDoubleColon);
		if (indexOfDoubleColon > 0) {
			totalNoOfColons = noOfColon - 1;
			for (var colonCounter = totalNoOfColons; colonCounter <= TOTAL_COLON_COUNT; colonCounter++) {
				zeroesToAppendStr += ':0000';
			}
			if (indexOfDoubleColon + 2 != rawInputAddress.length) {
				zeroesToAppendStr += ':'
			} else {
				zeroesToAppendStr += ':0000'
			}
		}
		//console.log('zeroesToAppendStr : '+zeroesToAppendStr);
		toReturnArray = rawInputAddress.replace('::', zeroesToAppendStr).split(':');
	}


	//console.log('Input address array : '+toReturnArray);
	for (var rawInputAddressIndex = 0; rawInputAddressIndex < toReturnArray.length; rawInputAddressIndex++) {
		if (toReturnArray[rawInputAddressIndex].length != 4) {
			toReturnArray[rawInputAddressIndex] = formatValue(toReturnArray[rawInputAddressIndex], 4, '0');
		} else {
			toReturnArray[rawInputAddressIndex] = toReturnArray[rawInputAddressIndex];
		}
	}
	return toReturnArray;
}

function formatValue(value, mandatoryBits, defaultValue) {
	var noOfBitsMissing = mandatoryBits - value.length;
	//console.log('value : '+value+', mandatoryBits : '+mandatoryBits+', defaultValue : '+defaultValue);
	return Array(noOfBitsMissing + 1).join(defaultValue) + value;
	//return 0;
}

//Converts IPv6 in the form '1111110100110000:0:0:1:1111111101001110:111110:1001:1110' to '1111110100110000:0000000000000000:0000000000000000:0000000000000001:1111111101001110:0000000000111110:0000000000001001:0000000000001110'
function formatBinaryAddress(binaryArray) {
	for (var binaryArrayIndex = 0; binaryArrayIndex < binaryArray.length; binaryArrayIndex++) {
		if (binaryArray[binaryArrayIndex].length != 16) {
			binaryArray[binaryArrayIndex] = formatValue(binaryArray[binaryArrayIndex], 16, '0');
		}
	}
	return binaryArray;
}


//Converts IPv6 in the form 'fd30:0000:0000:0001:ff4e:003e:0009:000e' to binary '1111110100110000:0000000000000000:0000000000000000:0000000000000001:1111111101001110:0000000000111110:0000000000001001:0000000000001110'
function convertHexToBinary(hexArray) {
	var toReturnArray = new Array(0, 0, 0, 0, 0, 0, 0, 0);
	for (var hexArrayIndex = 0; hexArrayIndex < hexArray.length; hexArrayIndex++) {
		toReturnArray[hexArrayIndex] = parseInt(hexArray[hexArrayIndex], 16).toString(2);
	}
	return formatBinaryAddress(toReturnArray);
}

//Converts IPv6 in the binary form '1111110100110000:0000000000000000:0000000000000000:0000000000000001:1111111101001110:0000000000111110:0000000000001001:0000000000001110' to 'fd30:0000:0000:0001:ff4e:003e:0009:000e' 
function convertBinaryToHex(binaryArray) {
	var toReturnArray = new Array(0, 0, 0, 0, 0, 0, 0, 0);
	for (var binaryToHexIndex = 0; binaryToHexIndex < binaryArray.length; binaryToHexIndex++) {
		toReturnArray[binaryToHexIndex] = parseInt(binaryArray[binaryToHexIndex], 2).toString(16);
	}
	return formatHexAddress(toReturnArray);
}

function convertBinaryAddressStringToBinaryAddressArray(binaryAddressString) {
	var toReturnArray = new Array(0, 0, 0, 0, 0, 0, 0, 0);
	var counter = 0
	////console.log('Binary address string : '+binaryAddressString);
	for (var binaryAddressStringIndex = 0; binaryAddressStringIndex < toReturnArray.length; binaryAddressStringIndex++) {
		toReturnArray[binaryAddressStringIndex] = binaryAddressString.substr(counter, WORD);
		////console.log('binaryAddressStringIndex : '+binaryAddressStringIndex+' : '+counter+' : '+WORD+' : '+binaryAddressString.substr(counter, WORD))
		counter += WORD;

	}
	return toReturnArray;
}

function convertHexAddressStringToHexAddressArray(hexAddressString) {
	var toReturnArray = new Array(0, 0, 0, 0, 0, 0, 0, 0);
	var counter = 0
	////console.log('Hex address string : '+hexAddressString);
	for (var hexAddressStringIndex = 0; hexAddressStringIndex < toReturnArray.length; hexAddressStringIndex++) {
		toReturnArray[hexAddressStringIndex] = hexAddressString.substr(counter, HEX_BLOCK_DIGITS);
		////console.log('binaryAddressStringIndex : '+binaryAddressStringIndex+' : '+counter+' : '+WORD+' : '+binaryAddressString.substr(counter, WORD))
		counter += HEX_BLOCK_DIGITS;

	}
	return toReturnArray;
}

function getNumberOfSubnets() {
	return Math.pow(2, SUBNET_BITS);
}

function getPossibleNoOfSubnetList() {
	var arrToReturn = new Array();
	//console.log('SUBNET_LIST_START_INDEX : '+SUBNET_LIST_START_INDEX+', SUBNET_LIST_END_INDEX : '+SUBNET_LIST_END_INDEX);
	for (var subnetBitIndex = SUBNET_LIST_START_INDEX; subnetBitIndex <= SUBNET_LIST_END_INDEX; subnetBitIndex++) {
		arrToReturn.push(new Array(subnetBitIndex, Math.pow(2, subnetBitIndex)));
	}
	return arrToReturn;
}

function setDefaultIPv6Values() {
	var networkbits = DEFAULT_NETWORK_BITS,
		maskbits = 0,
		subnetbits = 0,
		hostbits = 0
	ipv6AddressBlock = '2400:ca00::/56',  //No I18N
		addressBlockArr = null,
		ipv6Address = null;
	addressBlockArr = ipv6AddressBlock.split('/');
	ipv6Address = addressBlockArr[0];
	setNetworkAddress(formatHexAddress(ipv6Address));
	networkbits = parseInt(addressBlockArr[1], 10);
	maskbits = networkbits;
	subnetbits = maskbits - networkbits;
	setNetworkBits(networkbits);
	setHostBits(TOTAL_NETWORK_BITS - maskbits);
	setMaskBits(maskbits);
	setSubnetBits(subnetbits);
	setSubnetListStartIndex(0);
	setSubnetListEndIndex(TOTAL_NETWORK_BITS);
	setSubnetList(document.getElementById('ipv6SubnetBitsVsNoOfSubnets'));
	document.getElementById('ipv6AddressBlock').value = ipv6AddressBlock;
}

function setSubnetList(elementId) {
	var possibleNoOfSubnetList = 0;
	clearOptions(elementId);
	possibleNoOfSubnetList = getPossibleNoOfSubnetList();
	var newArrayList = [];
	for (var i = 0; i < possibleNoOfSubnetList.length; i++) {
		////console.log('possibleNoOfSubnetList[i][0] : '+possibleNoOfSubnetList[i][0]);
		if ((parseInt(possibleNoOfSubnetList[i][0]) + getNetworkBits()) <= TOTAL_NETWORK_BITS) {
			if (parseInt(possibleNoOfSubnetList[i][0]) == 0) {
				addOption(elementId, '/' + (parseInt(possibleNoOfSubnetList[i][0]) + getNetworkBits()) + ' (' + possibleNoOfSubnetList[i][1] + ' subnet)', possibleNoOfSubnetList[i][0]);
				newArrayList.push('/' + (parseInt(possibleNoOfSubnetList[i][0]) + getNetworkBits()) + ' (' + possibleNoOfSubnetList[i][1] + ' subnet)');
			} else {
				addOption(elementId, '/' + (parseInt(possibleNoOfSubnetList[i][0]) + getNetworkBits()) + ' (' + possibleNoOfSubnetList[i][1] + ' subnets)', possibleNoOfSubnetList[i][0]);
				newArrayList.push('/' + (parseInt(possibleNoOfSubnetList[i][0]) + getNetworkBits()) + ' (' + possibleNoOfSubnetList[i][1] + ' subnets)');
			}
		}


	}
	//console.log('setSubnetList : SUBNET_BITS : '+SUBNET_BITS);
	selectOption(elementId, SUBNET_BITS);
	return newArrayList;
}

function getInputValues() {
	var toReturnMap = new Array();
	var ipv6AddressBlock = null,
		ipv6Address = null,
		ipv6NetworkBits = null,
		ipv6SubnetBits = null;

	var subnetBitsElement = null,
		addressBlockArr = null;

	ipv6AddressBlock = document.getElementById('ipv6AddressBlock').value;
	subnetBitsElement = document.getElementById('ipv6SubnetBitsVsNoOfSubnets');

	if (ipv6AddressBlock == '') {
		return false;
	}
	if (ipv6AddressBlock.indexOf('/') == -1) {
		ipv6AddressBlock = ipv6AddressBlock + '/8';
	}
	addressBlockArr = ipv6AddressBlock.split('/');
	if (addressBlockArr[1] == '') {
		addressBlockArr[1] = 8;
		ipv6AddressBlock = ipv6AddressBlock + '8';
	}
	ipv6Address = addressBlockArr[0];
	if (parseInt(addressBlockArr[1]) > TOTAL_NETWORK_BITS) {
		ipv6NetworkBits = TOTAL_NETWORK_BITS;
		ipv6AddressBlock = ipv6AddressBlock.replace('/' + addressBlockArr[1], '/' + TOTAL_NETWORK_BITS);
	} else {
		ipv6NetworkBits = parseInt(addressBlockArr[1], 10);
	}
	ipv6SubnetBits = subnetBitsElement.options[subnetBitsElement.selectedIndex].value;
	//console.log('addressBlockArr : '+addressBlockArr);
	//console.log('ipv6Address : '+ipv6Address);
	//console.log('ipv6NetworkBits : '+ipv6NetworkBits);
	//console.log('ipv6SubnetBits : '+ipv6SubnetBits);

	toReturnMap.ipv6AddressBlock = ipv6AddressBlock;
	toReturnMap.ipv6Address = ipv6Address;
	toReturnMap.ipv6SubnetBits = parseInt(ipv6SubnetBits, 10);
	toReturnMap.ipv6NetworkBits = ipv6NetworkBits;
	////console.log('Input values map : '+toReturnMap);
	return toReturnMap;
}

function calculateNetworkAddressListForSubnetRange(fromSubnetId, toSubnetId) {
	var networkAddressList = new Array();
	var binaryNetworkAddress = 0,
		binaryNetworkBits = 0,
		binarySubnetBits = 0,
		binaryHostBits = 0;
	//console.log('fromSubnetId : '+fromSubnetId+',    toSubnetId : '+toSubnetId);
	fromSubnetId = new BigNumber(fromSubnetId, 10);
	toSubnetId = new BigNumber(toSubnetId, 10);
	counter = 0;

	binaryNetworkBits = convertHexToBinary(formatHexAddress(networkAddress.join(':'))).join('').substr(0, NETWORK_BITS);
	binarySubnetBits = Array(SUBNET_BITS + 1).join(0);
	binaryHostBits = Array((HOST_BITS) + 1).join(0);
	////console.log('Binary network bits : '+binaryNetworkBits+', Length : '+binaryNetworkBits.length);
	////console.log('Binary subnet bits : '+binarySubnetBits+', Length : '+binarySubnetBits.length);
	////console.log('Binary host bits : '+binaryHostBits+', Length : '+binaryHostBits.length);
	networkAddressArrayCounter = 0;
	//for(var subnetRangeIndex=fromSubnetId; subnetRangeIndex<=toSubnetId; subnetRangeIndex++){
	for (var subnetRangeIndex = fromSubnetId; subnetRangeIndex.lte(toSubnetId);) {
		var missingBinarySubnetBits = 0,
			tempSubnetBinaryBits = 0;
		tempSubnetBinaryBits = subnetRangeIndex.toString(2);
		lengthOfSubnetBinaryBits = tempSubnetBinaryBits.length;
		if (lengthOfSubnetBinaryBits < SUBNET_BITS) {
			missingBinarySubnetBits = Array((SUBNET_BITS - lengthOfSubnetBinaryBits) + 1).join(0);
			binarySubnetBits = missingBinarySubnetBits + tempSubnetBinaryBits;
		} else {
			binarySubnetBits = tempSubnetBinaryBits;
		}

		//console.log('binaryNetworkBits 	: '+binaryNetworkBits);
		//console.log('binarySubnetBits 	: '+binarySubnetBits);
		//console.log('binaryHostBits 	: '+binaryHostBits);
		binaryNetworkAddress = binaryNetworkBits.toString() + binarySubnetBits.toString() + binaryHostBits.toString();
		var formattedHexAddress = convertBinaryToHex(convertBinaryAddressStringToBinaryAddressArray(binaryNetworkAddress));
		//console.log('Binary network address for Subnet Id : '+subnetRangeIndex+' : '+convertBinaryAddressStringToBinaryAddressArray(binaryNetworkAddress)+', length : '+binaryNetworkAddress.length);
		//console.log('Formatted Hex address for Subnet Id : '+subnetRangeIndex+' : '+formattedHexAddress);
		networkAddressList[networkAddressArrayCounter] = [subnetRangeIndex, condenseHexAddress(formattedHexAddress.join(':'), false), getNetworkAddressRange(formattedHexAddress, MASK_BITS, SUBNET_BITS), getCidrNotation(formattedHexAddress), subnetRangeIndex.plus(1)];
		networkAddressArrayCounter += 1;
		subnetRangeIndex = subnetRangeIndex.plus(1);
	}
	return networkAddressList;
}

function calculateAndUpdateValues(action) {
	var addressBlock = null;
	var totalbits = 0,
		networkbits = 0,
		maskbits = 0,
		hostbits = 0,
		subnetbits = 0;
	var inputValuesMap = getInputValues();
	var networkAddressArray = null;

	setNetworkAddress(formatHexAddress(inputValuesMap.ipv6Address));
	if (action == 'setAddress') {
		networkbits = parseInt(inputValuesMap.ipv6NetworkBits, 10);
		subnetbits = 0;
		maskbits = networkbits;
		hostbits = TOTAL_BITS - maskbits
	} else if (action == 'setNoOfSubnets') { //No I18N
		networkbits = parseInt(inputValuesMap.ipv6NetworkBits, 10);
		subnetbits = inputValuesMap.ipv6SubnetBits;
		maskbits = networkbits + subnetbits;
		hostbits = TOTAL_BITS - maskbits
	}
	setNetworkBits(networkbits);
	setMaskBits(maskbits);
	setSubnetBits(subnetbits);
	setHostBits(hostbits);
	//console.log('Network bits : '+getNetworkBits()+', Mask bits : '+getMaskBits()+', subnet bits : '+getSubnetBits()+', host bits : '+getHostBits());
	var noOfSubnets = getNumberOfSubnets();
	if (noOfSubnets > MAX_SIZE_OF_SUBNET) {
		networkAddressArray = calculateNetworkAddressListForSubnetRange(0, MAX_SIZE_OF_SUBNET - 1);
		//console.log('Number of subnets : '+noOfSubnets);
		var dottedArray = getDottedArray();
		var networkAddressEndArray = calculateNetworkAddressListForSubnetRange((noOfSubnets - MAX_SIZE_OF_SUBNET), noOfSubnets - 1);
		networkAddressArray = networkAddressArray.concat(dottedArray).concat(networkAddressEndArray);
	} else {
		networkAddressArray = calculateNetworkAddressListForSubnetRange(0, noOfSubnets - 1);
	}

	////console.log('No of Subnets : '+getNumberOfSubnets());
	////console.log('Possible no of Subnets List : '+getPossibleNoOfSubnetList());
	var optionData = updateIPv6Values(inputValuesMap);
	var tableData = createTable(networkAddressArray);
	var newResult = [];
	newResult.push(optionData);
	newResult.push(tableData);
	return newResult
}

function getDottedArray() {
	var toReturnArray = new Array();
	var sizeOfDottedArray = 2;
	for (var indexOfDottedArr = 0; indexOfDottedArr < sizeOfDottedArray; indexOfDottedArr++) {
		toReturnArray[indexOfDottedArr] = ['..', '..', '..', '..', '..'];
	}
	return toReturnArray;
}

function updateIPv6Values(inputValuesMap) {
	//setSubnetListStartIndex(0);
	//setSubnetListEndIndex(TOTAL_NETWORK_BITS - getMaskBits());
	document.getElementById("ipv6AddressBlock").value = inputValuesMap.ipv6AddressBlock; //No I18N
	return setSubnetList(document.getElementById('ipv6SubnetBitsVsNoOfSubnets'));
}

function getCidrNotation(formattedHexAddress) {
	return condenseHexAddress(formattedHexAddress.join(':'), true) + '/' + getMaskBits();
}

function getSubnetAddress(address, maskbits, subnetbits) {
	var binarySubnetAdd = new Array(TOTAL_BITS);
	var binaryNetAddress = convertHexToBinary(formatHexAddress(address)).join('');
	var binaryMaskAddress = getBinaryMaskAddress(maskbits, subnetbits);
	var hexSubnetAddress = 0;
	for (var binaryNetworkAddressIndex = 0; binaryNetworkAddressIndex < binaryNetAddress.length; binaryNetworkAddressIndex++) {
		binarySubnetAdd[binaryNetworkAddressIndex] = (parseInt(binaryNetAddress[binaryNetworkAddressIndex], 2) & parseInt(binaryMaskAddress[binaryNetworkAddressIndex], 2)).toString();
		////console.log(binaryNetworkAddressIndex+' '+binaryNetAddress[binaryNetworkAddressIndex]+' & '+binaryMaskAddress[binaryNetworkAddressIndex]);
	}
	////console.log('Binary network address		: '+binaryNetAddress+', Type : '+typeof(binaryNetAddress)+', Length  : '+binaryNetAddress.length);
	////console.log('Binary mask address 		: '+binaryMaskAddress+', Type : '+typeof(binaryMaskAddress)+', Length  : '+binaryMaskAddress.length);
	////console.log('Binary subnet address		: '+binarySubnetAdd.join(''));
	hexSubnetAddress = convertBinaryToHex(convertBinaryAddressStringToBinaryAddressArray(binarySubnetAdd.join('')));
	////console.log('Hex subnet address : '+hexSubnetAddress);
	return hexSubnetAddress;
}

function getBinaryMaskAddress(maskbits, subnetbits) {
	//console.log('Get binary mask address , Maskbits : '+maskbits+' : '+subnetbits);
	var networkbits = Array(maskbits + subnetbits + 1).join(1);
	var hostbits = Array((HOST_BITS + 1)).join(0);
	////console.log('Binary mask address : '+networkbits+hostbits);
	return networkbits + hostbits;
}

function getHexMaskAddress(maskbits, subnetbits) {
	return convertBinaryToHex(convertBinaryAddressStringToBinaryAddressArray(getBinaryMaskAddress(maskbits, subnetbits)));
}

function getbroadcastAddress(address, maskbits) {
	var networkbits = convertHexToBinary(formatHexAddress(address)).join('').substr(0, maskbits);
	var hostbits = Array((TOTAL_HOST_BITS - maskbits) + 1).join(1);
	var hexBroadcastAdd = convertBinaryToHex(convertBinaryAddressStringToBinaryAddressArray(networkbits + hostbits));
	////console.log('Hex broadcast address : '+hexBroadcastAdd);
	return hexBroadcastAdd;
}

function calculateEndIpAddress(address, maskbits) {
	var hexEndIpAddress = null;
	var binaryBroadcastAddress = convertHexToBinary(getbroadcastAddress(address, maskbits)).join('');
	binaryBroadcastAddress = new BigNumber(binaryBroadcastAddress, 2);
	binaryOne = new BigNumber('1', 2);
	//hexEndIpAddress = binaryBroadcastAddress.minus(binaryOne).toString(16);
	hexEndIpAddress = binaryBroadcastAddress.toString(16);
	//console.log('binary Subnet address : '+binaryBroadcastAddress);
	//console.log('hexEndIpAddress : '+hexEndIpAddress);
	return convertHexAddressStringToHexAddressArray(hexEndIpAddress);
}

function calculateStartIpAddress(address, maskbits, subnetbits) {
	var hexStartIpAddress = null;
	var binarySubnetAddress = convertHexToBinary(getSubnetAddress(address, maskbits, subnetbits)).join('');
	binarySubnetAddress = new BigNumber(binarySubnetAddress, 2);
	binaryOne = new BigNumber('1', 2);
	//hexStartIpAddress = binarySubnetAddress.plus(binaryOne).toString(16);
	hexStartIpAddress = binarySubnetAddress.toString(16);
	//console.log('binary Subnet address : '+binarySubnetAddress);
	//console.log('hexStartIpAddress : '+hexStartIpAddress);
	return convertHexAddressStringToHexAddressArray(hexStartIpAddress);
}

function getNetworkAddressRange(address, maskbits, subnetbits) {
	return condenseHexAddress(calculateStartIpAddress(address, maskbits, subnetbits).join(':'), true) + ' - ' + condenseHexAddress(calculateEndIpAddress(address, maskbits).join(':'), true);
}

function createTable(resultArray) {
	//console.log('Result array length : '+resultArray.length);
	row = new Array();
	cell = new Array();
	row_num = resultArray.length + 1;
	cell_num = 4;
	divElement = document.createElement('div');
	divElement.setAttribute('class', 'table-responsive');
	tableElement = document.createElement('table');
	tableElement.setAttribute('id', 'newtable');
	// tableElement.setAttribute('width','935px');
	tableElement.setAttribute('class', 'table table-bordered table-stripped');
	divElement.setAttribute('id', 'newDiv');
	if (resultArray.length < 5) {
		// divElement.setAttribute('style','height: auto;width:950px');
	} else {
		// divElement.setAttribute('style','height: 400px;overflow:auto;width:950px');
	}
	tableBodyElement = document.createElement('tbody');
	for (c = 0; c < row_num; c++) {
		row[c] = document.createElement('tr');
		for (k = 0; k < cell_num; k++) {
			cell[k] = document.createElement('td');
			/*
			if(c!=0){
				//console.log('Value of resultArray[c-1][0] '+resultArray[c-1][0]);
			}
			*/
			if (c == 0) {
				cell[k].setAttribute('class', 'dynamicTableCell');
				cell[k].setAttribute('style', 'font-weight:bold;');
				row[c].setAttribute('class', 'bg-banner-solid--blue sticky-header')

				if (k == 0) {

				} else {
					// cell[k].setAttribute('style','font-weight:bold;width:253px;line-height:40px;');
				}

				switch (k) {
					case 0:
						cont = document.createTextNode('Subnet ID'); //No I18N
						break;
					case 1:
						cont = document.createTextNode('Subnet Address'); //No I18N
						break;
					case 2:
						cont = document.createTextNode('Host Address Range'); //No I18N
						break;
					case 3:
						cont = document.createTextNode('Notation'); //No I18N
						break;
				}
			} else {
				cell[k].setAttribute('class', 'dynamicTableCell');
				if (k == 0) {
					// cell[k].setAttribute('style','width:140px;line-height:40px;');
				} else if (k != 2) {
					// cell[k].setAttribute('style','width:253px;line-height:40px;');
				} else {
					// cell[k].setAttribute('style','width:253px;');
				}
				switch (k) {
					case 0:
						cont = document.createTextNode(resultArray[c - 1][4]);
						break;
					case 1:
						cont = document.createTextNode(resultArray[c - 1][1]);
						break;
					case 2:
						//console.log('resultArray[c-1][2] : '+resultArray[c-1][2].length);
						if (resultArray[c - 1][2].length >= 50) {
							// cell[k].setAttribute('style','width:253px;');
						} else {
							// cell[k].setAttribute('style','width:253px;line-height:40px;');
						}
						cont = document.createTextNode(resultArray[c - 1][2]);
						break;
					case 3:
						cont = document.createTextNode(resultArray[c - 1][3]);
						break;
				}
			}
			cell[k].appendChild(cont);
			row[c].appendChild(cell[k]);
		}
		tableBodyElement.appendChild(row[c]);
	}
	tableElement.appendChild(tableBodyElement);
	divElement.appendChild(tableElement);
	document.getElementById('tableDiv').innerHTML = '';
	document.getElementById("SubnetInfoDiv").style.display = "block";
	$("#promo-wrapper").show();
	document.getElementById('tableDiv').appendChild(divElement);
	//document.getElementById('tableDiv').innerHTML = "<pre style='word-wrap: normal;white-space: pre;font-weight: bold;font-size:13px;color:#0B610B;'>"+tableElement.innerHTML+"</pre>";

	return resultArray

}

function clearOptions(elementId) {
	var length = elementId.options.length;
	for (var i = length - 1; i >= 0; i--) {
		elementId.remove(i);
	}
}

function selectOption(elementId, value) {
	for (var i = 0; i < elementId.length; i++) {
		if (elementId[i].value == value) {
			////console.log("selecting : "+elementId[i].value+"   "+value);
			elementId.selectedIndex = i;
			break;
		}
	}
}

function addOption(elementId, text, value) {
	var optionElement = document.createElement('option');
	optionElement.text = text;
	optionElement.value = value;
	try {
		elementId.add(optionElement, null);
	} catch (e) {
		elementId.add(optionElement);
	}
}

function testForLoop() {
	startIndex = new BigNumber(36028797018962944, 10);
	endIndex = new BigNumber(36028797018963970, 10);
	counter = 0;
	for (var i = startIndex; i.lt(endIndex);) {
		i = i.plus(1);
		counter += 1;
		//console.log('i : '+i+', : '+counter);
	}

}

function initialize() {
	var networkAddress = 'fd30::1:ff4e:3e:9:e'; //No I18N
	var totalbits = 128,
		networkbits = 8,
		maskbits = 56,
		subnetbits = 0,
		hostbits = 0;
	setNetworkAddress(formatHexAddress(networkAddress));
	setTotalBits(totalbits);
	setNetworkBits(networkbits);
	setMaskBits(maskbits);
	setSubnetBits(maskbits - networkbits);
	setHostBits(totalbits - maskbits);
	/*
	//console.log('Network Address 	: '+getNetworkAddress());
	//console.log('Total Bits 		: '+getTotalBits());
	//console.log('Mask Bits			: '+getMaskBits());
	//console.log('Subnet Bits 		: '+getSubnetBits());
	//console.log('Host Bits 			: '+getHostBits());
	//console.log('Network Bits 		: '+getNetworkBits());
	//console.log('IPv6 address in Hex(formatted) : '+formatHexAddress(networkAddress));
	//console.log('IPv6 address in binary : '+convertHexToBinary(formatHexAddress(networkAddress)));
	//console.log('IPv6 address in binary(formatted) : '+convertHexToBinary(formatHexAddress(networkAddress)));
	//console.log('Converting formatted binary IPv6 address to Hex  : '+convertBinaryToHex(convertHexToBinary(formatHexAddress(networkAddress))));
	*/

	////console.log('No of Hosts : '+getNumberOfHosts());
	////console.log('Possible no of Hosts List : '+getPossibleNoOfHostsList());
	////console.log('No of Subnets : '+getNumberOfSubnets());
	////console.log('Possible no of Subnets List : '+getPossibleNoOfSubnetList());
}


function main() {
	initialize();
	//testForLoop();
}