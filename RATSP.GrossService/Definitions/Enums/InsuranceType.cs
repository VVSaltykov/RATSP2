using System.ComponentModel;
using RATSP.GrossService.Definitions.Attributes;

namespace RATSP.GrossService.Definitions.Enums;

using System;

public enum InsuranceType
{
    [InsuranceType("ИЮЛ/PD/MB/BI", "страхование имущества, перерыв в производстве")]
    PropertyLegalEntitiesFireAndOtherHazardsBreakdownBusinessInterruption,

    [InsuranceType("ИЮЛ/PD", "страхование имущества, юридические лица")]
    PropertyLegalEntitiesFireAndOtherHazards,

    [InsuranceType("ИЮЛ/PD/MB/EEI", "страхование имущества, электронные устройства")]
    PropertyLegalEntitiesFireAndOtherHazardsBreakdownElectronicDevices,

    [InsuranceType("ИЮЛ/PD/BI", "страхование имущества, перерыв в производстве")]
    PropertyLegalEntitiesFireAndOtherHazardsBusinessInterruption,

    [InsuranceType("ИФЛ", "страхование имущества, физические лица")]
    PropertyIndividualsExcludingVehicles,

    [InsuranceType("СМР", "строительно-монтажные риски")]
    ConstructionAndAssemblyRisks,

    [InsuranceType("ЖД", "железнодорожный транспорт")]
    RailwayVehicles,

    [InsuranceType("ГО", "страхование ответственности")]
    LiabilityInsurance,

    [InsuranceType("ИЮЛ/PD", "страхование имущества юридических лиц от огня и других опасностей")]
    PropertyLegalEntitiesFireAndOtherHazardsOnly,

    [InsuranceType("ИЮЛ/PD/MB", "страхование имущества, машины и оборудование от поломок")]
    PropertyLegalEntitiesFireAndOtherHazardsWithMachineryBreakdown,

    [InsuranceType("ИЮЛ/PD/MB/EEI", "страхование имущества, машины и оборудование от поломок, электронные устройства")]
    PropertyLegalEntitiesFireAndOtherHazardsWithMachineryBreakdownAndElectronicDevices,

    [InsuranceType("ИЮЛ/PD/BI", "страхование имущества, убытки от перерыва в производстве")]
    PropertyLegalEntitiesFireAndOtherHazardsWithBusinessInterruption,

    [InsuranceType("ИЮЛ/BI", "страхование убытков от перерыва в производстве")]
    PropertyLegalEntitiesBusinessInterruption,

    [InsuranceType("ИЮЛ/MB", "страхование машин и оборудования от поломок")]
    PropertyLegalEntitiesMachineryBreakdown,

    [InsuranceType("Груз", "страхование грузов")]
    CargoInsurance,

    [InsuranceType("ГО ОПО", "страхование ГО владельцев опасных производственных объектов")]
    LiabilityHazardousObjects,

    [InsuranceType("ГО ТЭК", "страхование ГО объектов топливно-энергетического комплекса")]
    LiabilityFuelEnergyComplex,

    [InsuranceType("ГО Air own", "страхование ГО владельцев аэропортов")]
    LiabilityAirportOwners,

    [InsuranceType("ИЮЛ/Авто", "страхование автомобильного транспорта юридических лиц")]
    LegalEntitiesAutoTransport
}

